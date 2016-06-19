using DehaxGL.Math;

namespace DehaxGL.Model
{
    public struct Vertex
    {
        private Vec3f _position;

        public double X { get { return _position.X; } }
        public double Y { get { return _position.Y; } }
        public double Z { get { return _position.Z; } }

        public Vec3f Position { get { return _position; } }

        public Vertex(double x, double y, double z)
        {
            _position = new Vec3f(x, y, z);
        }

        public Vertex(Vec3f position)
        {
            _position = position;
        }
    }
}
