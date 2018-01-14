using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using ProtoChain.TestApp.Managers;
using Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace ProtoChain.TestApp
{
    class Program
    {
        private static List<Peer> peerList = new List<Peer>();
        private static readonly NodeListManager nodeListManager = new NodeListManager();

        //static void Main(string[] args)
        //{
        //    //Get list of peers
        //    peerList.Add(new IPEndPoint(IPAddress.Parse("192.168.0.29"), 25555));

        //    Application app = new Application(25555, 5, 1, "log.log");
        //}

        private static Application _app;
        private static ManualResetEventSlim _exitSignal = new ManualResetEventSlim();

        static void Main(string[] args)
        {
            ConfigureLogging(Level.Info);

            var port = 25555;
            var maxConnections = 5;
            var statusInterval = 10;
            var logFile = "numbers.log";

            var localAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var localAddress in localAddresses)
            {
                nodeListManager.AddNodeToList(localAddress);
            }

            Run(port, maxConnections, statusInterval, logFile);
        }

        private static void ConfigureLogging(Level level)
        {
            var repository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var appender = new ConsoleAppender
            {
                Layout = new PatternLayout("%message%newline")
            };
            ((Hierarchy)repository).Root.Level = level;
            BasicConfigurator.Configure(repository, appender);
        }

        private static void Run(int port, int maxConnections, int statusInterval, string logFile)
        {
            Console.WriteLine("Note: Press <CTRL-C> to stop server.");

            // Create and run application, which does all the real work.
            _app = new Application(
                port, maxConnections, statusInterval,
                Path.Combine(Directory.GetCurrentDirectory(), logFile), nodeListManager);
            _app.Run(TerminateCommandReceived);

            Console.CancelKeyPress += delegate {
                Console.WriteLine("<CTRL-C> received.");
                StopServer();
                _exitSignal.Set();
            };


            //Get peers from a DNS
            var ipAddresses = Dns.GetHostAddresses("protochain.org");

            foreach (var peerIPAddress in ipAddresses)
            {
                try
                {
                    Console.WriteLine($"Trying to connect to peer with IP: '{peerIPAddress.ToString()}'");
                    var peer = new Peer(peerIPAddress);

                    peer.Connect();
                    var nodeList = peer.GetNodeList();
                    nodeListManager.SyncNodeList(nodeList);

                    Console.WriteLine($"Sent data to peer with IP: '{peerIPAddress.ToString()}'");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while trying to connect to peer with IP: '{peerIPAddress.ToString()}'");
                }
                
            }

            // Block on exit signal to keep process running until exit event encountered
            _exitSignal.Wait();
        }

        private static void StopServer()
        {
            Console.WriteLine("Stopping server...");
            _app.Dispose();
        }

        private static void TerminateCommandReceived()
        {
            Console.WriteLine("Terminate command received.");
            StopServer();
            _exitSignal.Set();
        }
    }
}