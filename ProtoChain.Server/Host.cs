using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProtoChain.Network;

namespace ProtoChain.Server
{
    public static class Host
    {
        public static Task StartWebHostAndShutdownBlockingTask(string[] args, Action<IServiceCollection> configureServices)
        {
             var webHost = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://*:25555")
                .ConfigureServices(configureServices)
                .Build();

            webHost.Start();

            return webHost.WaitForShutdownAsync();
        }
    }
}
