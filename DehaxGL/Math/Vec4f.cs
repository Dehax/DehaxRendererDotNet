namespace DehaxGL.Math
{
    public struct Vec4f
    {
        public double X;
        public double Y;
        public double Z;
        public double W;

        public Vec4f(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vec4f(Vec3f v3)
        {
            X = v3.X;
            Y = v3.Y;
            Z = v3.Z;
            W = 1.0;
        }

        public static Vec4f operator *(Vec4f v, Matrix a)
        {
            double rx = a[0, 0] * v.X + a[1, 0] * v.Y + a[2, 0] * v.Z + a[3, 0] * v.W;
            double ry = a[0, 1] * v.X + a[1, 1] * v.Y + a[2, 1] * v.Z + a[3, 1] * v.W;
            double rz = a[0, 2] * v.X + a[1, 2] * v.Y + a[2, 2] * v.Z + a[3, 2] * v.W;
            double rw = a[0, 3] * v.X + a[1, 3] * v.Y + a[2, 3] * v.Z + a[3, 3] * v.W;

            return new Vec4f(rx, ry, rz, rw);
        }

        public static Vec4f operator +(Vec4f a, Vec4f b)
        {
            Vec3f v1 = new Vec3f(a);
            Vec3f v2 = new Vec3f(b);

            return new Vec4f(v1 + v2);
        }
    }
}
