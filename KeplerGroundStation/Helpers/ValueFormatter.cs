using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeplerGroundStation.Helpers
{
    public static class ValueFormatter
    {
        public static String FormatDistance(double distance)
        {
            if (distance >= 1)
            {
                // Value in kilometers.
                return distance.ToString("#0.000") + " km";
            }
            else
            {
                // Value in meters.
                return ((int)(distance * 1000)) + " m";
            }
        }
    }
}
