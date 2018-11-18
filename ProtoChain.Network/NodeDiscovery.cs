using Microsoft.Extensions.Logging;
using ProtoChain.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoChain.Network
{
    public class NodeDiscovery : INodeDiscovery
    {
        private readonly ILogger<NodeDiscovery> _logger;

        public NodeDiscovery(ILogger<NodeDiscovery> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<IPAddress>> DiscoverNodes()
        {
            //Get the master nodes from the DNS
            var dnsIPAddresses = await Dns.GetHostAddressesAsync("protochain.org");

            //Try register local instance with master

            //Ask each DNS node for it's node list
            var getNodesTasks = new List<Task<IEnumerable<string>>>();
            foreach (var ipAddress in dnsIPAddresses)
            {
                getNodesTasks.Add(GetNodesFromNodeAsync(ipAddress));
            }

            var getNodesResults = await Task.WhenAll(getNodesTasks);
            return getNodesResults.SelectMany(x => x.Select(ipString => IPAddress.Parse(ipString))).Distinct().ToList();
        }

        private async Task<IEnumerable<string>> GetNodesFromNodeAsync(IPAddress ipAddress)
        {
            var client = new ProtoChainNodeClient(ipAddress, 25555, _logger);

            IEnumerable<string> nodes;
            try
            {
                nodes = await client.GetNodes();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetNodes request failed for IP Address: {ipAddress}", ipAddress);
                nodes = new List<string>();
            }

            return nodes;
        }

        public static IEnumerable<IPAddress> GetLocalIPAddresses()
        {
            return Dns.GetHostAddresses(Dns.GetHostName());
        }
    }
}
