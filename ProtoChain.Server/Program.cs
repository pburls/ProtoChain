﻿using System;
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
    public class Program
    {
        private static NodeListManager _nodeListManager = new NodeListManager();

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://*:25555")
                .ConfigureServices(ConfigureServices)
                .Build();

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<INodeListManager>(_nodeListManager);
        }
    }
}