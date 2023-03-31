using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeplerGroundStation.Helpers
{
    public class AngleCalculator
    {
        public static int CalculateTiltAngle(double ay, double az)
        {
            double angle = Math.Atan2(az, ay) * 180.0 / Math.PI;
            double realAngle = Math.Abs(angle + 90);
            return (int)realAngle;
        }
    }
}
