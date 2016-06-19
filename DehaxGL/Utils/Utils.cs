namespace DehaxGL.Utils
{
    public class Utils
    {
        public static double DegreeToRadian(double angle)
        {
            return System.Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / System.Math.PI);
        }
    }
}
