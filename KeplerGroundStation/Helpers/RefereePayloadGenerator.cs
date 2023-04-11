using KeplerGroundStation.Model;
using System;

namespace KeplerGroundStation.Helpers
{
    public class RefereePayloadGenerator
    {
        public static byte[] GeneratePayload(PayloadData payload)
        {
            Random rand = new Random();

            // Generate an empty array of 78 bytes.
            byte[] data = new byte[78];

            data[0] = 0xFF; // Constant
            data[1] = 0xFF; // Constant
            data[2] = 0x54; // Constant
            data[3] = 0x52; // Constant

            data[4] = 0x00; // Takim ID = 0
            data[5] = (byte)payload.PackageId; // Sayac degeri


            // Altitude data
            byte[] altitudeBytes = BitConverter.GetBytes(payload != null ? (float)payload.Altitude : 0f);

            data[6] = altitudeBytes[0];
            data[7] = altitudeBytes[1];
            data[8] = altitudeBytes[2];
            data[9] = altitudeBytes[3];

            // Rocket GPS Altitude
            // byte[] rocketGPSAltitude = BitConverter.GetBytes(payload.Altitude);
            data[10] = 0xFF;
            data[11] = 0xFF;
            data[12] = 0xFF;
            data[13] = 0xFF;

            // Rocket GPS Latitude
            byte[] rocketGPSLatBytes = BitConverter.GetBytes(payload != null ? (float)payload.GpsLat : 0f);
            data[14] = rocketGPSLatBytes[0];
            data[15] = rocketGPSLatBytes[1];
            data[16] = rocketGPSLatBytes[2];
            data[17] = rocketGPSLatBytes[3];

            // Rocket GPS Latitude
            byte[] rocketGPSLongBytes = BitConverter.GetBytes(payload != null ? (float)payload.GpsLong : 0f);
            data[18] = rocketGPSLongBytes[0];
            data[19] = rocketGPSLongBytes[1];
            data[20] = rocketGPSLongBytes[2];
            data[21] = rocketGPSLongBytes[3];

            // Payload GPS Altitude
            // byte[] payloadGPSAltitude = BitConverter.GetBytes(payload.Altitude);
            data[22] = 0xFF;
            data[23] = 0xFF;
            data[24] = 0xFF;
            data[25] = 0xFF;

            // Payload GPS Latitude
            byte[] payloadGPSLatBytes = BitConverter.GetBytes(payload != null ? (float)payload.GpsLat : 0f);
            data[26] = payloadGPSLatBytes[0];
            data[27] = payloadGPSLatBytes[1];
            data[28] = payloadGPSLatBytes[2];
            data[29] = payloadGPSLatBytes[3];

            // Payload GPS Latitude
            byte[] payloadGPSLongBytes = BitConverter.GetBytes(payload != null ? (float)payload.GpsLong : 0f);
            data[30] = payloadGPSLongBytes[0];
            data[31] = payloadGPSLongBytes[1];
            data[32] = payloadGPSLongBytes[2];
            data[33] = payloadGPSLongBytes[3];

            // Stage GPS Altitude
            data[34] = 0xFF;
            data[35] = 0xFF;
            data[36] = 0xFF;
            data[37] = 0xFF;

            // Stage GPS Latitude
            data[38] = 0xFF;
            data[39] = 0xFF;
            data[40] = 0xFF;
            data[41] = 0xFF;

            // Stage GPS Longitude
            data[42] = 0xFF;
            data[43] = 0xFF;
            data[44] = 0xFF;
            data[45] = 0xFF;

            // Gyro X
            byte[] gyroXBytes = BitConverter.GetBytes(payload != null ? (float)payload.GyroX : 0f);
            data[46] = gyroXBytes[0];
            data[47] = gyroXBytes[1];
            data[48] = gyroXBytes[2];
            data[49] = gyroXBytes[3];

            // Gyro Y
            byte[] gyroYBytes = BitConverter.GetBytes(payload != null ? (float)payload.GyroY : 0f);
            data[50] = gyroYBytes[0];
            data[51] = gyroYBytes[1];
            data[52] = gyroYBytes[2];
            data[53] = gyroYBytes[3];

            // Gyro Z
            byte[] gyroZBytes = BitConverter.GetBytes(payload != null ? (float)payload.GyroZ : 0f);
            data[54] = gyroZBytes[0];
            data[55] = gyroZBytes[1];
            data[56] = gyroZBytes[2];
            data[57] = gyroZBytes[3];

            // Accel X
            byte[] accelXBytes = BitConverter.GetBytes(payload != null ? (float)payload.AccelerationX : 0f);
            data[58] = accelXBytes[0];
            data[59] = accelXBytes[1];
            data[60] = accelXBytes[2];
            data[61] = accelXBytes[3];

            // Accel Y
            byte[] accelYBytes = BitConverter.GetBytes(payload != null ? (float)payload.AccelerationY : 0f);
            data[62] = accelYBytes[0];
            data[63] = accelYBytes[1];
            data[64] = accelYBytes[2];
            data[65] = accelYBytes[3];

            // Accel Z
            byte[] accelZBytes = BitConverter.GetBytes(payload != null ? (float)payload.AccelerationZ : 0f);
            data[66] = accelZBytes[0];
            data[67] = accelZBytes[1];
            data[68] = accelZBytes[2];
            data[69] = accelZBytes[3];

            // Tilt Angle
            float tiltAngle = AngleCalculator.CalculateTiltAngle(payload != null ? payload.AccelerationY : 0f, payload != null ? payload.AccelerationZ : 0f);
            byte[] tiltAngleBytes = BitConverter.GetBytes(payload != null ? tiltAngle : 0f);
            data[70] = tiltAngleBytes[0];
            data[71] = tiltAngleBytes[1];
            data[72] = tiltAngleBytes[2];
            data[73] = tiltAngleBytes[3];

            // Mission Status
            data[74] = 0x01; // Neither of the parachutes are ejected

            // Checksum
            data[75] = CalculateChecksum(data);

            data[76] = 0x0D; // Constant
            data[77] = 0x0A; // Constant

            return data;
        }

        private static byte CalculateChecksum(byte[] payload)
        {
            int checksum = 0;

            for (int i = 4; i < 75; i++)
            {
                checksum += payload[i];
            }
            return (byte)(checksum % 256);
        }
    }
}
