using KeplerGroundStation.Model;
using System;

namespace KeplerGroundStation.Helpers
{
    internal class PayloadParser
    {
        public static PayloadData ParsePayloadData(byte[] incomingData)
        {
            short deviceId = BitConverter.ToInt16(incomingData, 2);
            short packageId = BitConverter.ToInt16(incomingData, 4);
            short flightStatus = BitConverter.ToInt16(incomingData, 6);
            double temperature = BitConverter.ToSingle(incomingData, 8);
            double altitude = BitConverter.ToSingle(incomingData, 12);
            double pressure = BitConverter.ToSingle(incomingData, 16) / 100.0;
            double gpsLat = BitConverter.ToSingle(incomingData, 20);
            double gpsLng = BitConverter.ToSingle(incomingData, 24);
            double gpsAlt = BitConverter.ToSingle(incomingData, 28);
            double accelx = BitConverter.ToSingle(incomingData, 32);
            double accely = BitConverter.ToSingle(incomingData, 36);
            double accelz = BitConverter.ToSingle(incomingData, 40);
            double gyrox = BitConverter.ToSingle(incomingData, 44);
            double gyroy = BitConverter.ToSingle(incomingData, 48);
            double gyroz = BitConverter.ToSingle(incomingData, 52);

            return new PayloadData(deviceId, packageId, flightStatus, temperature, altitude, pressure, gpsLat, gpsLng, gpsAlt, accelx, accely, accelz, gyrox, gyroy, gyroz);
        }
    }
}
