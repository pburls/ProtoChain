using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ProtoChain.Network
{
    public interface INodeDiscovery
    {
        Task<IEnumerable<IPAddress>> DiscoverNodes();
    }
}