using System;

namespace KeplerGroundStation.Helpers
{
    public class AngleCalculator
    {
        public static float CalculateTiltAngle(double ay, double az)
        {
            double angle = Math.Atan2(az, ay) * 180.0 / Math.PI;
            double realAngle = Math.Abs(angle + 90);
            return (float)realAngle;
        }
    }
}
