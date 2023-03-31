using KeplerGroundStation.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeplerGroundStation.Helpers
{
    internal class PayloadParser
    {
        public static PayloadData ParsePayloadData(String incomingData)
        {
            string[] data = incomingData.Split(",");
            int packageId = int.Parse(data[0]);
            double altitude = double.Parse(data[1], CultureInfo.InvariantCulture);
            double pressure = double.Parse(data[2], CultureInfo.InvariantCulture) / 100.0;
            double temperature = double.Parse(data[3], CultureInfo.InvariantCulture);
            double gpsLat = double.Parse(data[4], CultureInfo.InvariantCulture);
            double gpsLong = double.Parse(data[5], CultureInfo.InvariantCulture);
            double accelx = double.Parse(data[6], CultureInfo.InvariantCulture);
            double accely = double.Parse(data[7], CultureInfo.InvariantCulture);
            double accelz = double.Parse(data[8], CultureInfo.InvariantCulture);
            double gyrox = double.Parse(data[9], CultureInfo.InvariantCulture);
            double gyroy = double.Parse(data[10], CultureInfo.InvariantCulture);
            double gyroz = double.Parse(data[11], CultureInfo.InvariantCulture);

            return new PayloadData(packageId, temperature, altitude, pressure, gpsLat, gpsLong, accelx, accely, accelz, gyrox, gyroy, gyroz);
        }
    }
}
