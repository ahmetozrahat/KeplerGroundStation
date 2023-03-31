using System;

namespace KeplerGroundStation.Helpers
{
    internal class DistanceCalculator
    {
        private static long R = 6371;

        /// <summary>
        /// Returns the distance in kilometers of any two
        /// latitude / longitude points.
        /// </summary>
        /// <param name="Lat1"></param>
        /// <param name="Long1"></param>
        /// <param name="Lat2"></param>
        /// <param name="Long2"></param>
        /// <returns></returns>
        public static double GetDistanceDifference(double Lat1, double Long1, double Lat2, double Long2)
        {
            var latitude = (Lat2 - Lat1).ToRadians();
            var longitude = (Long2 - Long1).ToRadians();
            var h1 = Math.Sin(latitude / 2) * Math.Sin(latitude / 2) +
                  Math.Cos(Lat1.ToRadians()) * Math.Cos(Lat2.ToRadians()) *
                  Math.Sin(longitude / 2) * Math.Sin(longitude / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return R * h2;

        }
    }
}
