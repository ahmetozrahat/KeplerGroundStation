namespace KeplerGroundStation.Model
{
    public class PayloadComputerPayloadData
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

        private double _temperature;
        public double Temperature
        {
            get { return _temperature; }
            set { _temperature = value; }
        }

        private double _humidity;
        public double Humidity
        {
            get { return _humidity; }
            set
            {
                _humidity = value;
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

        public PayloadComputerPayloadData(int DeviceId, int PackageId, double Temperature, double Humidity, double Pressure, double GpsLat, double GpsLng, double GpsAlt)
        {
            this.DeviceId = DeviceId;
            this.PackageId = PackageId;
            this.Temperature = Temperature;
            this.Humidity = Humidity;
            this.Pressure = Pressure;
            this.GpsLat = GpsLat;
            this.GpsLng = GpsLng;
            this.GpsAlt = GpsAlt;
        }
    }
}
