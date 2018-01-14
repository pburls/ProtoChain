using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoChain.TestApp.Managers
{
    public class NodeListManager
    {
        private List<string> NodeList { get; set; }

        public NodeListManager()
        {
            NodeList = new List<string>();
        }

        public string[] GetNodeList ()
        {
            return NodeList.ToArray();
        }


        public void SyncNodeList(string[] NodeAddressList)
        {
            foreach (var nodeAddress in NodeAddressList)
            {
                AddNodeToList(nodeAddress);
            }
        }

        public void AddNodeToList(string IPV4Address)
        {
            if (ValidateIPv4(IPV4Address) && !NodeList.Contains(IPV4Address))
            {
                NodeList.Add(IPV4Address);
            }
                    }

        private bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
    }
}
