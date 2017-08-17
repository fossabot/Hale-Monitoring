namespace Hale.Lib
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    public static class Networking
    {
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }

        public static List<string> GetHostIPs(AddressFamily type = AddressFamily.InterNetwork)
        {
            List<string> addresses = new List<string>();

            IPHostEntry host = Dns.GetHostEntryAsync(Dns.GetHostName()).Result;
            host.AddressList.ToList().ForEach(ip =>
            {
                if (ip.AddressFamily == type)
                {
                    addresses.Add(ip.ToString());
                }
            });

            return addresses;
        }
    }
}
