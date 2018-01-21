using Microsoft.Extensions.DependencyInjection;
using ProtoChain.Network;
using ProtoChain.Server;
using System;
using System.Threading.Tasks;

namespace ProtoChain.Service
{
    class Program
    {
        private static NodeListManager _nodeListManager;
        private static ILocalPeer _localPeer;
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            //Create the node list
            _nodeListManager = new NodeListManager();

            //Start accepting API requests
            var webApiTask = Host.StartWebHostAndShutdownBlockingTask(args, ConfigureServices);

            //Start joining the P2P network
            _localPeer = _serviceProvider.GetRequiredService<ILocalPeer>();
            var peerTask = _localPeer.StartAsync();

            //Start 
            Task.WaitAll(new Task[] { webApiTask, peerTask });
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<INodeListManager>(_nodeListManager);
            services.AddTransient<ILocalPeer, LocalPeer>();
            services.AddTransient<INodeDiscovery, NodeDiscovery>();
            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
