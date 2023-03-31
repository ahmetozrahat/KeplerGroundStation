using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeplerGroundStation.Model
{
    public class PayloadData
    {
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

        private double _gpsLong;
        public double GpsLong
        {
            get { return _gpsLong; }
            set
            {
                _gpsLong = value;
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

        public PayloadData(int PackageId, double Temperature, double Altitude, double Pressure, double GpsLat, double GpsLong, double AccelerationX, double AccelerationY, double AccelerationZ, double GyroX, double GyroY, double GyroZ)
        {
            this.PackageId = PackageId;
            this.Temperature = Temperature;
            this.Altitude = Altitude;
            this.Pressure = Pressure;
            this.GpsLat = GpsLat;
            this.GpsLong = GpsLong;
            this.AccelerationX = AccelerationX;
            this.AccelerationY = AccelerationY;
            this.AccelerationZ = AccelerationZ;
            this.GyroX = GyroX;
            this.GyroY = GyroY;
            this.GyroZ = GyroZ;
        }
    }
}
