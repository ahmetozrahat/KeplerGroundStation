using KeplerGroundStation.Model;
using System;
using System.ComponentModel;

namespace KeplerGroundStation.Helpers
{
    public class RefereePayloadGenerator: INotifyPropertyChanged
    {
        private int _refereePayloadPackageId;
        /// <summary>
        /// Counter for Referee Computer payload package id.
        /// </summary>
        public int RefereePayloadPackageId
        {
            get { return _refereePayloadPackageId; }
            set
            {
                _refereePayloadPackageId = value;
                OnPropertyChanged(nameof(RefereePayloadPackageId));
            }
        }

        public RefereePayloadGenerator()
        {
            RefereePayloadPackageId = 0;
        }

        /// <summary>
        /// Generates the data for Referee Computer.
        /// </summary>
        /// <param name="flightComputerPayloadData"></param>
        /// <param name="backupComputerPayloadData"></param>
        /// <param name="payloadComputerPayloadData"></param>
        /// <returns></returns>
        public byte[] GeneratePayload(FlightComputerPayloadData flightComputerPayloadData, BackupComputerPayloadData backupComputerPayloadData, PayloadComputerPayloadData payloadComputerPayloadData)
        {
            // Generate an empty array of 78 bytes.
            byte[] data = new byte[78];

            data[0] = 0xFF; // Constant
            data[1] = 0xFF; // Constant
            data[2] = 0x54; // Constant
            data[3] = 0x52; // Constant

            data[4] = 0x00; // Takim ID = 0
            data[5] = CalculatePackageId(); // Sayac degeri

            // Altitude data
            byte[] altitudeBytes = BitConverter.GetBytes(flightComputerPayloadData != null ? (float)flightComputerPayloadData.Altitude : 0f);

            data[6] = altitudeBytes[0];
            data[7] = altitudeBytes[1];
            data[8] = altitudeBytes[2];
            data[9] = altitudeBytes[3];

            // Rocket GPS Altitude
            if (flightComputerPayloadData == null || (flightComputerPayloadData != null && flightComputerPayloadData.GpsAlt == 0))
            {
                // Flight computer payload data has no GPS Alt. Check if there is data in BackupFlightComputer.
                byte[] rocketGPSAltitude = BitConverter.GetBytes(backupComputerPayloadData != null ? (float)backupComputerPayloadData.GpsAlt : 0f);

                data[10] = rocketGPSAltitude[0];
                data[11] = rocketGPSAltitude[1];
                data[12] = rocketGPSAltitude[2];
                data[13] = rocketGPSAltitude[3];
            }
            else
            {
                // Flight computer has payload data.
                byte[] rocketGPSAltitude = BitConverter.GetBytes(flightComputerPayloadData != null ? (float)flightComputerPayloadData.GpsAlt : 0f);

                data[10] = rocketGPSAltitude[0];
                data[11] = rocketGPSAltitude[1];
                data[12] = rocketGPSAltitude[2];
                data[13] = rocketGPSAltitude[3];
            }

            // Rocket GPS Latitude
            if (flightComputerPayloadData == null || (flightComputerPayloadData != null && flightComputerPayloadData.GpsLat == 0))
            {
                // Flight computer payload data has no GPS Lat. Check if there is data in BackupFlightComputer.
                byte[] rocketGPSLatitude = BitConverter.GetBytes(backupComputerPayloadData != null ? (float)backupComputerPayloadData.GpsLat : 0f);

                data[14] = rocketGPSLatitude[0];
                data[15] = rocketGPSLatitude[1];
                data[16] = rocketGPSLatitude[2];
                data[17] = rocketGPSLatitude[3];
            }
            else
            {
                // Flight computer has payload data.
                byte[] rocketGPSLatitude = BitConverter.GetBytes(flightComputerPayloadData != null ? (float)flightComputerPayloadData.GpsLat : 0f);

                data[14] = rocketGPSLatitude[0];
                data[15] = rocketGPSLatitude[1];
                data[16] = rocketGPSLatitude[2];
                data[17] = rocketGPSLatitude[3];
            }

            // Rocket GPS Longitude
            if (flightComputerPayloadData == null || (flightComputerPayloadData != null && flightComputerPayloadData.GpsLng == 0))
            {
                // Flight computer payload data has no GPS Lng. Check if there is data in BackupFlightComputer.
                byte[] rocketGPSLongitude = BitConverter.GetBytes(backupComputerPayloadData != null ? (float)backupComputerPayloadData.GpsLng : 0f);

                data[18] = rocketGPSLongitude[0];
                data[19] = rocketGPSLongitude[1];
                data[20] = rocketGPSLongitude[2];
                data[21] = rocketGPSLongitude[3];
            }
            else
            {
                // Flight computer has payload data.
                byte[] rocketGPSLongitude = BitConverter.GetBytes(flightComputerPayloadData != null ? (float)flightComputerPayloadData.GpsLng : 0f);

                data[18] = rocketGPSLongitude[0];
                data[19] = rocketGPSLongitude[1];
                data[20] = rocketGPSLongitude[2];
                data[21] = rocketGPSLongitude[3];
            }

            // Payload GPS Altitude
            byte[] payloadGPSAltitude = BitConverter.GetBytes(payloadComputerPayloadData != null ? (float)payloadComputerPayloadData.GpsAlt : 0f);

            data[22] = payloadGPSAltitude[0];
            data[23] = payloadGPSAltitude[1];
            data[24] = payloadGPSAltitude[2];
            data[25] = payloadGPSAltitude[3];

            // Payload GPS Latitude
            byte[] payloadGPSLatBytes = BitConverter.GetBytes(payloadComputerPayloadData != null ? (float)payloadComputerPayloadData.GpsLat : 0f);
            data[26] = payloadGPSLatBytes[0];
            data[27] = payloadGPSLatBytes[1];
            data[28] = payloadGPSLatBytes[2];
            data[29] = payloadGPSLatBytes[3];

            // Payload GPS Latitude
            byte[] payloadGPSLongBytes = BitConverter.GetBytes(payloadComputerPayloadData != null ? (float)payloadComputerPayloadData.GpsLng : 0f);
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
            byte[] gyroXBytes = BitConverter.GetBytes(flightComputerPayloadData != null ? (float)flightComputerPayloadData.GyroX : 0f);
            data[46] = gyroXBytes[0];
            data[47] = gyroXBytes[1];
            data[48] = gyroXBytes[2];
            data[49] = gyroXBytes[3];

            // Gyro Y
            byte[] gyroYBytes = BitConverter.GetBytes(flightComputerPayloadData != null ? (float)flightComputerPayloadData.GyroY : 0f);
            data[50] = gyroYBytes[0];
            data[51] = gyroYBytes[1];
            data[52] = gyroYBytes[2];
            data[53] = gyroYBytes[3];

            // Gyro Z
            byte[] gyroZBytes = BitConverter.GetBytes(flightComputerPayloadData != null ? (float)flightComputerPayloadData.GyroZ : 0f);
            data[54] = gyroZBytes[0];
            data[55] = gyroZBytes[1];
            data[56] = gyroZBytes[2];
            data[57] = gyroZBytes[3];

            // Accel X
            byte[] accelXBytes = BitConverter.GetBytes(flightComputerPayloadData != null ? (float)flightComputerPayloadData.AccelerationX : 0f);
            data[58] = accelXBytes[0];
            data[59] = accelXBytes[1];
            data[60] = accelXBytes[2];
            data[61] = accelXBytes[3];

            // Accel Y
            byte[] accelYBytes = BitConverter.GetBytes(flightComputerPayloadData != null ? (float)flightComputerPayloadData.AccelerationY : 0f);
            data[62] = accelYBytes[0];
            data[63] = accelYBytes[1];
            data[64] = accelYBytes[2];
            data[65] = accelYBytes[3];

            // Accel Z
            byte[] accelZBytes = BitConverter.GetBytes(flightComputerPayloadData != null ? (float)flightComputerPayloadData.AccelerationZ : 0f);
            data[66] = accelZBytes[0];
            data[67] = accelZBytes[1];
            data[68] = accelZBytes[2];
            data[69] = accelZBytes[3];

            // Tilt Angle
            float tiltAngle = AngleCalculator.CalculateTiltAngle(flightComputerPayloadData != null ? flightComputerPayloadData.AccelerationY : 0f, flightComputerPayloadData != null ? flightComputerPayloadData.AccelerationZ : 0f);
            byte[] tiltAngleBytes = BitConverter.GetBytes(flightComputerPayloadData != null ? tiltAngle : 0f);
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

        /// <summary>
        /// Calculates the checksum of the payload data to be sent.
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        private byte CalculateChecksum(byte[] payload)
        {
            int checksum = 0;

            for (int i = 4; i < 75; i++)
            {
                checksum += payload[i];
            }
            return (byte)(checksum % 256);
        }

        /// <summary>
        /// Increments the package id counter in the range of 0-255.
        /// </summary>
        /// <returns></returns>
        private byte CalculatePackageId()
        {
            if (RefereePayloadPackageId == 255)
            {
                RefereePayloadPackageId = 0;
                return (byte)RefereePayloadPackageId;
            }else
            {
                RefereePayloadPackageId++;
                return (byte)RefereePayloadPackageId;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
