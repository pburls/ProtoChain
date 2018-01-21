using Microsoft.Extensions.Logging;
using ProtoChain.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProtoChain.Service
{
    public class LocalPeer : ILocalPeer
    {
        private readonly INodeListManager nodeListManager;
        private readonly INodeDiscovery nodeDiscovery;
        private readonly ILogger<LocalPeer> logger;

        public LocalPeer(INodeListManager nodeListManager, INodeDiscovery nodeDiscovery, ILogger<LocalPeer> logger)
        {
            this.nodeListManager = nodeListManager;
            this.nodeDiscovery = nodeDiscovery;
            this.logger = logger;
        }

        public async Task StartAsync()
        {
            //Discover other nodes
            var nodeIPAddresses = await nodeDiscovery.DiscoverNodes();

            nodeListManager.AddNodes(nodeIPAddresses);

            logger.LogInformation(nodeListManager.ToString());
        }
    }
}
