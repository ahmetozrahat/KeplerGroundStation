using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeplerGroundStation.Model
{
    class KeplerBaudRate
    {
        private static List<int> _baudRates;
        public static List<int> BaudRates
        {
            get {
                return new List<int>
                {
                    300,
                    600,
                    1200,
                    2400,
                    4800,
                    9600,
                    19200,
                    38400,
                    115200
                };
            }
            set { _baudRates = value; }
        }
    }
}
