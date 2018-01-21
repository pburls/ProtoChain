using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProtoChain.Client
{
    public class ProtoChainNodeClient : IProtoChainNodeClient
    {
        private HttpClient httpClient;
        private readonly ILogger _logger;

        public ProtoChainNodeClient(IPAddress ipAddress, int port, ILogger logger)
        {
            _logger = logger;

            httpClient = new HttpClient();
            var uriBuilder = new UriBuilder("http", ipAddress.ToString(), port);
            httpClient.BaseAddress = uriBuilder.Uri;
        }

        public async Task<IEnumerable<string>> GetNodes()
        {
            _logger.LogInformation("Requesting GetNodes from node {address}", httpClient.BaseAddress);
            using (var result = await httpClient.GetAsync("api/nodes"))
            {
                return await result.Content.ReadAsAsync<IEnumerable<string>>();
            }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
