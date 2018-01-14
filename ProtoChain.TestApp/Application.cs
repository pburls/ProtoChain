using ProtoChain.TestApp.Managers;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Server
{
    public class Application : IDisposable
    {
        private readonly int _statusInterval;
        private readonly LocalhostSocketListener _listener;
        private readonly NodeListManager nodeListManager;

        public Application(int port, int maxConnections, int statusInterval, string logFilePath)
        {
            _statusInterval = statusInterval;
            
            _listener = new LocalhostSocketListener(port, maxConnections);

            nodeListManager = new NodeListManager();

            var localAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach(var localAddress in localAddresses)
            {
                nodeListManager.AddNodeToList(localAddress);
            }
        }

        public void Run(Action terminationCallback = null)
        {
            // Start listening on the specified port, and get callback whenever
            // new socket connection is established.
            _listener.Start(socket =>
            {
                // New connection. Start reading data from the network stream.
                // Socket stream reader will call back when a valid value is read
                // and/or when a terminate command is received.
                var reader = new SocketStreamReader(socket);
                reader.Handle(nodeListManager);
            });
        }

        public void Dispose()
        {
            _listener.Stop();
        }
    }
}
