namespace KeplerGroundStation.Helpers
{
    public class FlightStatusHelper
    {
        public static string GetFlightStatusString(int status)
        {
            switch (status)
            {
                case 1:
                    return "Paraşütler Açılmadı";
                case 2:
                    return "Birincil Paraşüt Açıldı, İkincil Paraşüt Açılmadı";
                case 3:
                    return "Birincil Paraşüt Açılmadı, İkincil Paraşüt Açıldı";
                case 4:
                    return "Paraşütler Açıldı";
                default:
                    return "";
            }
        }
    }
}
