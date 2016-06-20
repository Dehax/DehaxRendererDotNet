using DehaxGL.Math;
using DehaxGL.Model;
using System.Drawing;

namespace DehaxGL
{
    public class DehaxGL
    {
        public enum RenderMode { Wireframe = 0x1, Solid = 0x2, Both = Wireframe | Solid };

        private IViewport _viewport;
        private Scene _scene;
        private Camera _camera;
        private int[] _zBuffer;
        private int _width;
        private int _height;
        private bool _renderAxis;

        public Camera Camera { get { return _camera; } }
        public Scene Scene { get { return _scene; } }

        public DehaxGL(IViewport viewport)
        {
            _viewport = viewport;
            _zBuffer = null;
            _renderAxis = true;

            _scene = new Scene();
            _camera = new Camera();
            _width = _viewport.Width;
            _height = _viewport.Height;

            SetViewportSize(_width, _height);
        }

        public void SetViewportSize(int width, int height)
        {
            if (width == 0 || height == 0)
            {
                return;
            }

            _camera.Width = width;
            _camera.Height = height;
            _viewport.SetSize(width, height);
            _width = width;
            _height = height;

            _zBuffer = new int[width * height];
        }

        public void ToggleAxisRender()
        {
            _renderAxis = !_renderAxis;
        }

        private Matrix CalculateVertexMatrix(Matrix world)
        {
            return world * _camera.ViewMatrix * _camera.ProjectionMatrix;
        }

        private Vec3i CalculateScreenCoordinates(Vec3f v)
        {
            int depth = int.MaxValue / 2;

            int x = (int)((v.X + 1.0) * _width * 0.5);
            int y = (int)((v.Y + 1.0) * _height * 0.5);
            int z = (int)((v.X + 1.0) * -depth);

            return new Vec3i(x, y, z);
        }

        private void DrawLine(Vec3i from, Vec3i to, Color color)
        {
            int x0 = from.X;
            int y0 = from.Y;
            int x1 = to.X;
            int y1 = to.Y;

            bool steep = false;

            if (System.Math.Abs(x0 - x1) < System.Math.Abs(y0 - y1))
            {
                int temp = x0;
                x0 = y0;
                y0 = temp;

                temp = x1;
                x1 = y1;
                y1 = temp;

                steep = true;
            }

            if (x0 > x1)
            {
                int temp = x0;
                x0 = x1;
                x1 = temp;

                temp = y0;
                y0 = y1;
                y1 = temp;
            }

            int dx = x1 - x0;
            int dy = y1 - y0;
            int derror2 = System.Math.Abs(dy) * 2;
            int error2 = 0;
            int y = y0;

            for (int x = x0; x <= x1; x++)
            {
                if (steep)
                {
                    _viewport.SetPixel(y, x, color);
                }
                else
                {
                    _viewport.SetPixel(x, y, color);
                }

                error2 += derror2;

                if (error2 > dx)
                {
                    y += (y1 > y0 ? 1 : -1);
                    error2 -= dx * 2;
                }
            }
        }

        private void DrawTriangle(Vec3i t0, Vec3i t1, Vec3i t2, Color color, int[] zBuffer)
        {
            if (t0.Y == t1.Y && t0.Y == t2.Y)
            {
                return;
            }

            if (t0.Y > t1.Y)
            {
                Vec3i temp = t0;
                t0 = t1;
                t1 = temp;
            }

            if (t0.Y > t2.Y)
            {
                Vec3i temp = t0;
                t0 = t2;
                t2 = temp;
            }

            if (t1.Y > t2.Y)
            {
                Vec3i temp = t1;
                t1 = t2;
                t2 = temp;
            }

            int total_height = t2.Y - t0.Y;

            for (int i = 0; i < total_height; i++)
            {
                bool second_half = i > t1.Y - t0.Y || t1.Y == t0.Y;
                int segment_height = second_half ? t2.Y - t1.Y : t1.Y - t0.Y;
                double alpha = (double)i / total_height;
                double beta = (double)(i - (second_half ? t1.Y - t0.Y : 0)) / segment_height;
                Vec3i A = new Vec3i(new Vec3f(t0) + new Vec3f(t2 - t0) * alpha);
                Vec3i B = second_half ? new Vec3i(new Vec3f(t1) + new Vec3f(t2 - t1) * beta) : new Vec3i(new Vec3f(t0) + new Vec3f(t1 - t0) * beta);

                if (A.X > B.X)
                {
                    Vec3i temp = A;
                    A = B;
                    B = temp;
                }

                for (int j = A.X; j <= B.X; j++)
                {
                    double phi = B.X == A.X ? 1.0 : (double)(j - A.X) / (double)(B.X - A.X);
                    Vec3i P = new Vec3i(new Vec3f(A) + new Vec3f(B - A) * phi);

                    int idx = P.X + P.Y * _width;

                    if (idx >= 0 && idx < _width * _height && zBuffer[idx] < P.Z)
                    {
                        zBuffer[idx] = P.Z;
                        _viewport.SetPixel(P.X, P.Y, color);
                    }
                }
            }
        }

        private void DrawFace(Vec3f v1, Vec3f v2, Vec3f v3, Color triangleColor, Color edgeColor, int[] zBuffer, RenderMode renderMode, bool backfaceCulling)
        {
            Vec3i s1 = CalculateScreenCoordinates(v1);
            Vec3i s2 = CalculateScreenCoordinates(v2);
            Vec3i s3 = CalculateScreenCoordinates(v3);

            if (!backfaceCulling && (((int)(renderMode) & (int)RenderMode.Solid) > 0))
            {
                DrawTriangle(s1, s2, s3, triangleColor, zBuffer);
            }

            if ((((int)(renderMode) & (int)RenderMode.Wireframe) > 0))
            {
                DrawLine(s1, s2, edgeColor);
                DrawLine(s2, s3, edgeColor);
                DrawLine(s3, s1, edgeColor);
            }
        }

        private void RenderModel(Model.Model model, RenderMode renderMode)
        {
            Matrix worldMatrix = model.WorldMatrix;
            Matrix viewMatrix = _camera.ViewMatrix;
            Matrix projectionMatrix = _camera.ProjectionMatrix;

            Mesh mesh = model.Mesh;
            int numFaces = mesh.NumFaces;
            Color modelColor = model.Color;
            Color edgeColor = Color.White;

            Camera.ProjectionType projection = _camera.Projection;
            double viewDistance = _camera.ViewDistance;
            double zoom = _camera.Zoom;
            double fov = _camera.FOV;
            double nearZ = _camera.NearZ;
            double farZ = _camera.FarZ;

            double objectRadius = mesh.MaxLocalScale;
            Vec3f objectPosition = model.Position;
            Vec3f objectPositionView = new Vec3f(new Vec4f(objectPosition) * viewMatrix);
            double objectNearZ = objectPositionView.Z - objectRadius * model.Scale.Z;
            double objectFarZ = objectPositionView.Z + objectRadius * model.Scale.Z;
            double objectLeftX = objectPositionView.X - objectRadius * model.Scale.X;
            double objectRightX = objectPositionView.X + objectRadius * model.Scale.X;
            double objectBottomY = objectPositionView.Y - objectRadius * model.Scale.Y;
            double objectTopY = objectPositionView.Y + objectRadius * model.Scale.Y;

            double clipX = 0.0;
            double clipY = 0.0;

            if (objectNearZ <= nearZ || objectFarZ >= farZ)
            {
                return;
            }

            if (projection == Camera.ProjectionType.Parallel)
            {
                if (_width > _height)
                {
                    clipX = 0.5 * zoom;
                }
                else
                {
                    clipX = _width * 0.5 * zoom / _height;
                }
            }
            else if (projection == Camera.ProjectionType.Perspective)
            {
                clipX = 0.5 * _width * objectPositionView.Z / viewDistance;
                clipX = System.Math.Tan(fov / 2.0) * (_width / (double)_height) * objectPositionView.Z;
            }

            if (objectLeftX < -clipX || objectRightX > clipX)
            {
                return;
            }

            if (_camera.Projection == Camera.ProjectionType.Parallel)
            {
                if (_width > _height)
                {
                    clipY = _height * 0.5 * zoom / _width;
                }
                else
                {
                    clipY = 0.5 * zoom;
                }
            }
            else if (_camera.Projection == Camera.ProjectionType.Perspective)
            {
                clipY = 0.5 * _height * objectPositionView.Z / viewDistance;
                clipY = System.Math.Tan(fov / 2.0) * objectPositionView.Z;
            }

            if (objectBottomY < -clipY || objectTopY > clipY)
            {
                return;
            }

            Face face;
            Vertex v1, v2, v3;
            Vec3f local1, local2, local3;
            Vec4f world1, world2, world3;
            Vec4f view1, view2, view3;
            Vec3f view1v3, view2v3, view3v3;
            Vec4f result1, result2, result3;
            Vec3f hc1, hc2, hc3;
            Vec3f n;
            Vec3f lightDirection;
            bool backfaceCulling;

            for (int j = 0; j < numFaces; j++)
            {
                face = mesh.GetFace(j);

                v1 = mesh.GetVertex(face.v1);
                v2 = mesh.GetVertex(face.v2);
                v3 = mesh.GetVertex(face.v3);

                local1 = v1.Position;
                local2 = v2.Position;
                local3 = v3.Position;

                world1 = new Vec4f(local1) * worldMatrix;
                world2 = new Vec4f(local2) * worldMatrix;
                world3 = new Vec4f(local3) * worldMatrix;

                n = Vec3f.Cross(new Vec3f(world3) - new Vec3f(world1), new Vec3f(world2) - new Vec3f(world1));
                n = Vec3f.Normal(n);

                lightDirection = _camera.LookAt - _camera.Position;
                lightDirection = Vec3f.Normal(lightDirection);

                double intensity = -(n * lightDirection);

                if (intensity < 0.0)
                {
                    intensity = 0.0;
                }

                Color faceColor = Color.FromArgb((int)(modelColor.R * intensity), (int)(modelColor.G * intensity), (int)(modelColor.B * intensity));

                lightDirection = (projection == Camera.ProjectionType.Parallel ? _camera.LookAt : new Vec3f(world1)) - _camera.Position;
                lightDirection = Vec3f.Normal(lightDirection);
                intensity = -(n * lightDirection);

                if (intensity <= 0.0)
                {
                    backfaceCulling = true;
                }
                else
                {
                    backfaceCulling = false;
                }

                view1 = world1 * viewMatrix;
                view2 = world2 * viewMatrix;
                view3 = world3 * viewMatrix;

                view1v3 = new Vec3f(view1);
                view2v3 = new Vec3f(view2);
                view3v3 = new Vec3f(view3);

                result1 = view1 * projectionMatrix;
                result2 = view2 * projectionMatrix;
                result3 = view3 * projectionMatrix;

                hc1 = new Vec3f(result1);
                hc2 = new Vec3f(result2);
                hc3 = new Vec3f(result3);

                DrawFace(hc1, hc2, hc3, faceColor, edgeColor, _zBuffer, renderMode, backfaceCulling);
            }
        }

        public void Render(RenderMode renderMode)
        {
            _viewport.Lock();
            _viewport.Clear();

            int numObjects = _scene.NumObjects;

            for (int i = 0; i < _width * _height; i++)
            {
                _zBuffer[i] = int.MinValue;
            }

            for (int i = (_renderAxis ? 0 : 3); i < numObjects; i++)
            {
                Model.Model model = _scene[i];

                RenderModel(model, renderMode);
            }

            _viewport.Unlock();
        }
    }
}
