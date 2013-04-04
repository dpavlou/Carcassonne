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
                var address = Dns.GetHostAddresses(DNS)[0];
                IP = address.ToString();
            }
        }

        public string IP
        {
            get; 
            set;
        }
    }
}
