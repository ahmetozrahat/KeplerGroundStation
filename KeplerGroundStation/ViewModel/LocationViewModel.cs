using KeplerGroundStation.Helpers;
using Microsoft.Maps.MapControl.WPF;

namespace KeplerGroundStation.ViewModel
{
    public class LocationViewModel
    {
        public Location FlightComputerLocation { get; set; }
        public Pushpin FlightComputerPin { get; set; }

        public Location BackupComputerLocation { get; set; }
        public Pushpin BackupComputerPin { get; set; }

        public Location PayloadComputerLocation { get; set; }
        public Pushpin PayloadComputerPin { get; set; }

        public Location GroundStationLocation { get; set; }
        public Pushpin GroundStationPin { get; set; }

        public LocationViewModel()
        {
            FlightComputerPin = new Pushpin()
            {
                Background = KeplerColors.Red
            };

            BackupComputerPin = new Pushpin()
            {
                Background = KeplerColors.Green
            };

            PayloadComputerPin = new Pushpin()
            {
                Background = KeplerColors.Blue
            };

            GroundStationPin = new Pushpin()
            {
                Background = KeplerColors.Magenta
            };
        }

        public void UpdateFlightComputerLocation(double lat, double lng)
        {
            FlightComputerLocation = new Location(lat, lng);
        }

        public void UpdateBackupComputerLocation(double lat, double lng)
        {
            BackupComputerLocation = new Location(lat, lng);
        }

        public void UpdatePayloadComputerLocation(double lat, double lng)
        {
            PayloadComputerLocation = new Location(lat, lng);
        }

        public void UpdateGroundStationLocation(double lat, double lng)
        {
            GroundStationLocation = new Location(lat, lng);
        }

        public LocationCollection GetLocations()
        {
            LocationCollection locations = new();

            if (FlightComputerLocation != null && FlightComputerLocation.Latitude != 0 && FlightComputerLocation.Longitude != 0)
            {
                locations.Add(FlightComputerLocation);
            }

            if (BackupComputerLocation != null &&  BackupComputerLocation.Latitude != 0 && BackupComputerLocation.Longitude != 0)
            {
                locations.Add(BackupComputerLocation);
            }

            if (PayloadComputerLocation != null && PayloadComputerLocation.Latitude != 0 && PayloadComputerLocation.Longitude != 0)
            {
                locations.Add(PayloadComputerLocation);
            }

            if (GroundStationLocation != null && GroundStationLocation.Latitude != 0 && GroundStationLocation.Longitude != 0)
            {
                locations.Add(GroundStationLocation);
            }
            return locations;
        }
    }
}
