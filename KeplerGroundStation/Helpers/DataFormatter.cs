using System.Text;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace KeplerGroundStation.Helpers
{
    public class DataFormatter
    {
        public static string FormatAcceleration(double accel)
        {
            return accel.ToString("F", CultureInfo.InvariantCulture) + " m/s";
        }

        public static string FormatGyro(double gyro)
        {
            return gyro.ToString("F", CultureInfo.InvariantCulture) + " rad/s";
        }

        public static string FormatAngle(float angle)
        {
            return (int)angle + "°";
        }

        public static string FormatDistanceMeters(double meters)
        {
            return meters.ToString("F", CultureInfo.InvariantCulture) + " m";
        }

        public static string FormatPressure(double pressure)
        {
            return pressure.ToString("F", CultureInfo.InvariantCulture) + " hPa";
        }

        public static string FormatTemperature(double temperature)
        {
            return temperature.ToString("F", CultureInfo.InvariantCulture) + " °C";
        }

        public static string FormatHumidity(double temperature)
        {
            return "%" + temperature.ToString("F", CultureInfo.InvariantCulture);
        }

        public static string FormatByteArray(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            StringBuilder sb = new();

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.AppendFormat("{0:x2} ", bytes[i]);
            }

            return sb.ToString().Trim();
        }

        public static string FormatByteArray(List<byte> bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            StringBuilder sb = new();

            for (int i = 0; i < bytes.Count; i++)
            {
                sb.AppendFormat("{0:x2} ", bytes[i]);
            }

            return sb.ToString().Trim();
        }

        public static string FormatLocation(double location)
        {
            return location.ToString("#0.000000", CultureInfo.InvariantCulture);
        }

        public static string FormatDistance(double distance)
        {
            if (distance >= 1)
            {
                // Value in kilometers.
                return distance.ToString("#0.000", CultureInfo.InvariantCulture) + " km";
            }
            else
            {
                // Value in meters.
                return ((int)(distance * 1000)) + " m";
            }
        }
    }
}
