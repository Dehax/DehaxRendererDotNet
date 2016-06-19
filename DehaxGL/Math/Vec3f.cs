using System.Text;

namespace DehaxGL.Math
{
    public struct Vec3f
    {
        public double X;
        public double Y;
        public double Z;

        public double Length
        {
            get
            {
                return System.Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public Vec3f(double a)
        {
            X = a;
            Y = a;
            Z = a;
        }

        public Vec3f(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec3f(Vec3i v3i)
        {
            X = v3i.X;
            Y = v3i.Y;
            Z = v3i.Z;
        }

        public Vec3f(Vec4f v4)
        {
            X = v4.X / v4.W;
            Y = v4.Y / v4.W;
            Z = v4.Z / v4.W;
        }

        public static Vec3f Normal(Vec3f v3)
        {
            double length = v3.Length;

            return new Vec3f(v3.X / length, v3.Y / length, v3.Z / length);
        }

        public static Vec3f Cross(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.Z * b.Y - a.Y * b.Z, a.X * b.Z - a.Z * b.X, a.Y * b.X - a.X * b.Y);
        }

        public static Vec3f operator -(Vec3f v)
        {
            return new Vec3f(-v.X, -v.Y, -v.Z);
        }

        public static Vec3f operator -(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3f operator +(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static double operator *(Vec3f a, Vec3f b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static Vec3f operator *(Vec3f a, double b)
        {
            return new Vec3f(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vec3f operator /(Vec3f a, double b)
        {
            return new Vec3f(a.X / b, a.Y / b, a.Z / b);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(X.ToString("F2"));
            sb.Append(", ");
            sb.Append(Y.ToString("F2"));
            sb.Append(", ");
            sb.Append(Z.ToString("F2"));

            return sb.ToString();
        }
    }
}
