using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProtoChain.Client
{
    public interface IProtoChainNodeClient : IDisposable
    {
        Task<IEnumerable<string>> GetNodes();
        Task<bool> PutNode();
    }
}