namespace DehaxGL.Math
{
    public struct Vec3i
    {
        public int X;
        public int Y;
        public int Z;

        public double Length
        {
            get
            {
                return System.Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public Vec3i(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec3i(Vec3f v3f)
        {
            X = (int)(v3f.X + 0.5);
            Y = (int)(v3f.Y + 0.5);
            Z = (int)(v3f.Z + 0.5);
        }

        public static Vec3i operator -(Vec3i a, Vec3i b)
        {
            return new Vec3i(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
    }
}
