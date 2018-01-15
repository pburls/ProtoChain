using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ProtoChain.Network
{
    public class NodeListManager : INodeListManager
    {
        private HashSet<IPAddress> UniqueAddresses { get; set; }

        public NodeListManager()
        {
            UniqueAddresses = new HashSet<IPAddress>();
        }

        public IEnumerable<IPAddress> GetNodes()
        {
            return UniqueAddresses;
        }

        public void AddNodes(IEnumerable<IPAddress> addressList)
        {
            foreach (var nodeAddress in addressList)
            {
                AddNode(nodeAddress);
            }
        }

        public void AddNode(IPAddress ipAddress)
        {
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                UniqueAddresses.Add(ipAddress);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--Node List--");
            sb.AppendLine("-------------");
            foreach (var node in UniqueAddresses)
            {
                sb.AppendLine(node.ToString());
            }
            sb.AppendLine("-------------");
            return sb.ToString();
        }
    }
}
