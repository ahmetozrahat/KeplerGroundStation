namespace KeplerGroundStation.Model
{
    public class BackupComputerPayloadData
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

        public BackupComputerPayloadData(int DeviceId, int PackageId, double GpsLat, double GpsLng, double GpsAlt)
        {
            this.DeviceId = DeviceId;
            this.PackageId = PackageId;
            this.GpsLat = GpsLat;
            this.GpsLng = GpsLng;
            this.GpsAlt = GpsAlt;
        }
    }
}
