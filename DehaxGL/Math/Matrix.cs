namespace DehaxGL.Math
{
    public struct Matrix
    {
        //public double M11, M12, M13, M14;
        //public double M21, M22, M23, M24;
        //public double M31, M32, M33, M34;
        //public double M41, M42, M43, M44;

        public double[,] m;

        public double this[int i, int j]
        {
            get
            {
                return m[i, j];
            }
            set
            {
                m[i, j] = value;
            }
        }

        public Matrix(bool identity = false)
        {
            m = new double[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    m[i, j] = 0.0;
                }
            }

            if (identity)
            {
                for (int i = 0; i < 4; i++)
                {
                    m[i, i] = 1.0;
                }
            }
        }

        public static Matrix Transform(double x, double y, double z)
        {
            Matrix result = new Matrix(true);
            result[3, 0] = x;
            result[3, 1] = y;
            result[3, 2] = z;

            return result;
        }

        public static Matrix Transform(Vec3f offset)
        {
            return Transform(offset.X, offset.Y, offset.Z);
        }

        public static Matrix RotationX(double angle)
        {
            Matrix result = new Matrix(true);

            double cosAngle = System.Math.Cos(angle);
            double sinAngle = System.Math.Sin(angle);

            result[1, 1] = cosAngle;
            result[1, 2] = sinAngle;
            result[2, 1] = -sinAngle;
            result[2, 2] = cosAngle;

            return result;
        }

        public static Matrix RotationY(double angle)
        {
            Matrix result = new Matrix(true);

            double cosAngle = System.Math.Cos(angle);
            double sinAngle = System.Math.Sin(angle);

            result[0, 0] = cosAngle;
            result[0, 2] = -sinAngle;
            result[2, 0] = sinAngle;
            result[2, 2] = cosAngle;

            return result;
        }

        public static Matrix RotationZ(double angle)
        {
            Matrix result = new Matrix(true);

            double cosAngle = System.Math.Cos(angle);
            double sinAngle = System.Math.Sin(angle);

            result[0, 0] = cosAngle;
            result[0, 1] = sinAngle;
            result[1, 0] = -sinAngle;
            result[1, 1] = cosAngle;

            return result;
        }

        public static Matrix Rotation(double angleX, double angleY, double angleZ)
        {
            return RotationX(angleX) * RotationY(angleY) * RotationZ(angleZ);
        }

        public static Matrix Rotation(Vec3f rotation)
        {
            return Rotation(rotation.X, rotation.Y, rotation.Z);
        }

        public static Matrix Rotation(Vec3f rotation, Vec3f pivot)
        {
            return Transform(-pivot) * Rotation(rotation) * Transform(pivot);
        }

        public static Matrix Scale(double x, double y, double z)
        {
            Matrix result = new Matrix(true);
            result[0, 0] = x;
            result[1, 1] = y;
            result[2, 2] = z;

            return result;
        }

        public static Matrix Scale(Vec3f scale)
        {
            return Scale(scale.X, scale.Y, scale.Z);
        }

        public static Matrix Scale(Vec3f scale, Vec3f pivot)
        {
            return Transform(-pivot) * Scale(scale) * Transform(pivot);
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            Matrix result = new Matrix();
            double sum;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    sum = 0.0;

                    for (int k = 0; k < 4; k++)
                    {
                        sum += a.m[i, k] * b.m[k, j];
                    }

                    result.m[i, j] = sum;
                }
            }

            return result;
        }
    }
}
