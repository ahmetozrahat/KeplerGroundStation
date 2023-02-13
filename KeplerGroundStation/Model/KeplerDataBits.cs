using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeplerGroundStation.Model
{
    internal class KeplerDataBits
    {
        private static List<int> _dataBits;
        public static List<int> DataBits
        {
            get
            {
                return new List<int>
                {
                    5,
                    6,
                    7,
                    8,
                };
            }
            set { _dataBits = value; }
        }
    }
}
