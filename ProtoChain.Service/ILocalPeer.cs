using System.Threading.Tasks;

namespace ProtoChain.Service
{
    public interface ILocalPeer
    {
        Task StartAsync();
    }
}