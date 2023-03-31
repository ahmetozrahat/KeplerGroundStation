using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeplerGroundStation.Helpers
{
    public class DataFormatter
    {
        public static string formatAcceleration(double accel)
        {
            return accel + " m/s";
        }

        public static string formatGyro(double gyro)
        {
            return gyro + " rad/s";
        }

        public static string formatAngle(int angle)
        {
            return angle + "°";
        }

        public static string formatDistanceMeters(double meters)
        {
            return meters + " m";
        }

        public static string formatPressure(double pressure)
        {
            return pressure + " hPa";
        }

        public static string formatTemperature(double temperature)
        {
            return temperature + " °C";
        }
    }
}
