using DehaxGL.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DehaxGL
{
    public class Camera
    {
        public enum ProjectionType { Parallel, Perspective };

        private const ProjectionType DEFAULT_PROJECTION = ProjectionType.Perspective;
        private readonly Vec3f DEFAULT_POSITION = new Vec3f(0.0, 0.0, -200.0);
        private readonly Vec3f DEFAULT_LOOK_AT = new Vec3f(0.0, 0.0, 0.0);
        private readonly Vec3f DEFAULT_UP = new Vec3f(0.0, 1.0, 0.0);
        private const double DEFAULT_FOV = System.Math.PI / 2.0;
        private const double MIN_FOV = 20.0;
        private const double MAX_FOV = 150.0;
        private const int DEFAULT_PARALLEL_ZOOM = 500;
        private const double DEFAULT_NEAR_Z = 1.0;
        private const double DEFAULT_FAR_Z = 500.0;

        private int _width;
        private int _height;
        private int _zoom;

        private ProjectionType _projection;
        private Vec3f _position = new Vec3f();
        private Vec3f _lookAt = new Vec3f();
        private Vec3f _up = new Vec3f();
        private double _fov;
        private double _nearPlaneZ;
        private double _farPlaneZ;

        private double _theta;
        private double _phi;

        public double ViewDistance
        {
            get
            {
                return _width / (2 * System.Math.Tan(_fov / 2.0));
            }
            set
            {
                _fov = 2 * System.Math.Atan(0.5 * _width / value);
            }
        }

        public double FOV
        {
            get
            {
                return _fov;
            }
            set
            {
                if (value <= Utils.Utils.DegreeToRadian(MAX_FOV) && value >= Utils.Utils.DegreeToRadian(MIN_FOV))
                {
                    _fov = value;
                }
            }
        }

        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        public int Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = value;
            }
        }

        public ProjectionType Projection
        {
            get
            {
                return _projection;
            }
            set
            {
                _projection = value;
            }
        }

        public Vec3f Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public Vec3f LookAt
        {
            get
            {
                return _lookAt;
            }
            set
            {
                _lookAt = value;
            }
        }

        public Vec3f Up
        {
            get
            {
                return _up;
            }
            set
            {
                _up = value;
            }
        }

        public double NearZ
        {
            get
            {
                return _nearPlaneZ;
            }
            set
            {
                if (value >= 1.0 && value < _farPlaneZ)
                {
                    _nearPlaneZ = value;
                }
            }
        }

        public double FarZ
        {
            get
            {
                return _farPlaneZ;
            }
            set
            {
                if (value <= 1000.0 && value > _nearPlaneZ)
                {
                    _farPlaneZ = value;
                }
            }
        }

        public Matrix ViewMatrix
        {
            get
            {
                Vec3f zAxis = Vec3f.Normal(_lookAt - _position);
                Vec3f xAxis = Vec3f.Normal(Vec3f.Cross(zAxis, _up));
                Vec3f yAxis = Vec3f.Cross(xAxis, zAxis);

                Matrix viewMatrix = new Matrix();
                viewMatrix[0, 0] = xAxis.X;
                viewMatrix[1, 0] = xAxis.Y;
                viewMatrix[2, 0] = xAxis.Z;
                viewMatrix[3, 0] = -(xAxis * _position);
                viewMatrix[0, 1] = yAxis.X;
                viewMatrix[1, 1] = yAxis.Y;
                viewMatrix[2, 1] = yAxis.Z;
                viewMatrix[3, 1] = -(yAxis * _position);
                viewMatrix[0, 2] = zAxis.X;
                viewMatrix[1, 2] = zAxis.Y;
                viewMatrix[2, 2] = zAxis.Z;
                viewMatrix[3, 2] = -(zAxis * _position);
                viewMatrix[3, 3] = 1.0;

                return viewMatrix;
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                Matrix projection = new Matrix();

                double h;
                double v;
                double zf = _farPlaneZ;
                double zn = _nearPlaneZ;
                double aspectRatio = _width / (double)_height;
                double yScale = 1.0 / System.Math.Tan(_fov / 2.0);
                double xScale = yScale / aspectRatio;

                switch (_projection)
                {
                    case ProjectionType.Parallel:
                        if (_width > _height)
                        {
                            h = 0.5 * _zoom;
                            v = _height * h / _width;
                        }
                        else
                        {
                            v = 0.5 * _zoom;
                            h = _width * v / _height;
                        }
                        break;
                    case ProjectionType.Perspective:
                        projection[0, 0] = xScale;
                        projection[1, 1] = yScale;
                        projection[2, 2] = (zf + zn) / (zf - zn);
                        projection[2, 3] = 1.0;
                        projection[3, 2] = -2 * zf * zn / (zf - zn);
                        break;
                }

                return projection;
            }
        }

        public Camera()
        {
            _position = DEFAULT_POSITION;
            _lookAt = DEFAULT_LOOK_AT;
            _up = DEFAULT_UP;
            _fov = DEFAULT_FOV;
            _projection = DEFAULT_PROJECTION;
            _zoom = DEFAULT_PARALLEL_ZOOM;
            _nearPlaneZ = DEFAULT_NEAR_Z;
            _farPlaneZ = DEFAULT_FAR_Z;

            _theta = System.Math.PI / 2.0;
            _phi = 0.0;
        }

        public Camera(Vec3f position, Vec3f lookAt, Vec3f up, double FOV)
        {
            _position = position;
            _lookAt = lookAt;
            _up = up;
            _fov = FOV;
            _projection = DEFAULT_PROJECTION;
            _zoom = DEFAULT_PARALLEL_ZOOM;
            _nearPlaneZ = DEFAULT_NEAR_Z;
            _farPlaneZ = DEFAULT_FAR_Z;

            _theta = System.Math.PI / 2.0;
            _phi = 0.0;
        }

        public void Rotate(double angleX, double angleY, double angleZ)
        {
            Vec3f oldPosition = _position - _lookAt;
            double r = oldPosition.Length;

            if (Utils.Utils.RadianToDegree(_theta) >= 5.0 && Utils.Utils.RadianToDegree(_theta) <= 175.0)
            {
                _theta += angleX;
            }
            else
            {
                if (Utils.Utils.RadianToDegree(_theta) < 5.0)
                {
                    _theta = Utils.Utils.DegreeToRadian(5.0);
                }
                else
                {
                    _theta = Utils.Utils.DegreeToRadian(175.0);
                }
            }

            _phi += angleY;

            Vec3f newPosition = new Vec3f(r * System.Math.Sin(_theta) * System.Math.Sin(_phi), r * System.Math.Cos(_theta), -r * System.Math.Sin(_theta) * System.Math.Cos(_phi));

            _position = newPosition + _lookAt;
        }

        public void ZoomIn(double multiplier)
        {
            double zoom = System.Math.Pow(2.0, multiplier);

            switch (_projection)
            {
                case ProjectionType.Parallel:
                    _zoom = (int)(_zoom / zoom);
                    break;
                case ProjectionType.Perspective:
                    double newFOV = _fov - multiplier / Utils.Utils.DegreeToRadian(1.0);
                    FOV = _fov;
                    break;
            }
        }

        public void ChangeProjection()
        {
            if (_projection == ProjectionType.Perspective)
            {
                _projection = ProjectionType.Parallel;
            }
            else if (_projection == ProjectionType.Parallel)
            {
                _projection = ProjectionType.Perspective;
            }
        }

        public void Move(Vec3f offset)
        {
            _position += offset;
            _lookAt += offset;
        }

        public void StrafeRight(double value)
        {
            Vec3f zAxis = Vec3f.Normal(_lookAt - _position);
            Vec3f delta = zAxis * value;

            _position += delta;
            _lookAt += delta;
        }

        public void StrafeUp(double value)
        {
            Vec3f zAxis = Vec3f.Normal(_lookAt - _position);
            Vec3f xAxis = Vec3f.Normal(Vec3f.Cross(zAxis, _up));
            Vec3f yAxis = Vec3f.Cross(xAxis, zAxis);
            Vec3f delta = yAxis * value;

            _position += delta;
            _lookAt += delta;
        }
    }
}
