namespace ProtoChain.Network
{
    public interface INodeListManager
    {
        void AddNodeToList(string IPV4Address);
        string[] GetNodeList();
        void SyncNodeList(string[] NodeAddressList);
    }
}