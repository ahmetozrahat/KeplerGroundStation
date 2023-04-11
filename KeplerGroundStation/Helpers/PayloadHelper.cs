namespace KeplerGroundStation.Helpers
{
    public class PayloadHelper
    {
        public static int FindSequence(byte[] buffer, int length, byte[] sequence)
        {
            for (int i = 0; i < length - sequence.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < sequence.Length; j++)
                {
                    if (buffer[i + j] != sequence[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
