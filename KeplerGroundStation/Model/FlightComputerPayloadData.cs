namespace KeplerGroundStation.Model
{
    public class FlightComputerPayloadData
    {
        private int _deviceId;
        public int DeviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }

        private int _packageId;
        public int PackageId
        {
            get { return _packageId; }
            set { _packageId = value; }
        }

        private int _flightStatus;
        public int FlightStatus
        {
            get { return _flightStatus; }
            set { _flightStatus = value; }
        }

        private double _temperature;
        public double Temperature
        {
            get { return _temperature; }
            set { _temperature = value; }
        }

        private double _altitude;
        public double Altitude
        {
            get { return _altitude; }
            set
            {
                _altitude = value;
            }
        }

        private double _pressure;
        public double Pressure
        {
            get { return _pressure; }
            set
            {
                _pressure = value;
            }
        }


        private double _gpsLat;
        public double GpsLat
        {
            get { return _gpsLat; }
            set
            {
                _gpsLat = value;
            }
        }

        private double _gpsLng;
        public double GpsLng
        {
            get { return _gpsLng; }
            set
            {
                _gpsLng = value;
            }
        }

        private double _gpsAlt;
        public double GpsAlt
        {
            get { return _gpsAlt; }
            set
            {
                _gpsAlt = value;
            }
        }

        private double _accelerationX;
        public double AccelerationX
        {
            get { return _accelerationX; }
            set
            {
                _accelerationX = value;
            }
        }

        private double _accelerationY;
        public double AccelerationY
        {
            get { return _accelerationY; }
            set
            {
                _accelerationY = value;
            }
        }

        private double _accelerationZ;
        public double AccelerationZ
        {
            get { return _accelerationZ; }
            set
            {
                _accelerationZ = value;
            }
        }

        private double _gyroX;
        public double GyroX
        {
            get { return _gyroX; }
            set
            {
                _gyroX = value;
            }
        }

        private double _gyroY;
        public double GyroY
        {
            get { return _gyroY; }
            set
            {
                _gyroY = value;
            }
        }

        private double _gyroZ;
        public double GyroZ
        {
            get { return _gyroZ; }
            set
            {
                _gyroZ = value;
            }
        }

        public FlightComputerPayloadData(int DeviceId, int PackageId, int FlightStatus, double Temperature, double Altitude, double Pressure, double GpsLat, double GpsLng, double GpsAlt, double AccelerationX, double AccelerationY, double AccelerationZ, double GyroX, double GyroY, double GyroZ)
        {
            this.DeviceId = DeviceId;
            this.PackageId = PackageId;
            this.FlightStatus = FlightStatus;
            this.Temperature = Temperature;
            this.Altitude = Altitude;
            this.Pressure = Pressure;
            this.GpsLat = GpsLat;
            this.GpsLng = GpsLng;
            this.GpsAlt = GpsAlt;
            this.AccelerationX = AccelerationX;
            this.AccelerationY = AccelerationY;
            this.AccelerationZ = AccelerationZ;
            this.GyroX = GyroX;
            this.GyroY = GyroY;
            this.GyroZ = GyroZ;
        }
    }
}
