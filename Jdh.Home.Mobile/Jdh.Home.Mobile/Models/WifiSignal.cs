namespace Jdh.Home.Mobile.Models
{
    public class WifiSignal
    {
        public WifiSignal(string ssid, string capabilities, int level)
        {
            Ssid = ssid;
            Capabilities = capabilities;
            Level = level;
        }

        public string Ssid { get; }
        public string Capabilities { get; }
        public int Level { get; }

        public override string ToString()
        {
            return Ssid;
        }
    }
}
