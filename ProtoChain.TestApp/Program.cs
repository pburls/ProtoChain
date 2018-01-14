using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ProtoChain.TestApp
{
    class Program
    {
        private static List<EndPoint> peerList = new List<EndPoint>();

        static void Main(string[] args)
        {
            //Get list of peers
            peerList.Add(new IPEndPoint(IPAddress.Parse("192.168.0.29"), 25555));

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IPv4);
            socket.Connect(peerList[0]);
        }
    }
}