using System;
using System.Net;

namespace Client
{
    public class DnsToIP
    {
        public DnsToIP(string DNS)
        {
            if (char.IsDigit(DNS[0]))
                IP = DNS;
            else
            {
                try
                {
                    var address = Dns.GetHostAddresses(DNS)[0];
                    IP = address.ToString();
                }
                catch (System.Net.Sockets.SocketException exc)
                {
                    Console.WriteLine(exc.Message);
                    IP = "127.0.0.1";
                }

            }
        }

        public string IP
        {
            get; 
            set;
        }
    }
}
