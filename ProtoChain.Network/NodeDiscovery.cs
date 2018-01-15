using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ProtoChain.Network
{
    public static class NodeDiscovery
    {
        public static IEnumerable<IPAddress> GetLocalIPAddresses()
        {
            return Dns.GetHostAddresses(Dns.GetHostName());
        }

        public static IEnumerable<IPAddress> GetDNSIPAddresses(string dns)
        {
            return Dns.GetHostAddresses(dns);
        }
    }
}
