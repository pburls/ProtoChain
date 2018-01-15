using System.Collections.Generic;
using System.Net;

namespace ProtoChain.Network
{
    public interface INodeListManager
    {
        void AddNode(IPAddress ipAddress);
        IEnumerable<IPAddress> GetNodes();
        void AddNodes(IEnumerable<IPAddress> addressList);
    }
}