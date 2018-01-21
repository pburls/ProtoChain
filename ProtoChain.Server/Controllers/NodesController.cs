using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProtoChain.Network;

namespace ProtoChain.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/Nodes")]
    public class NodesController : Controller
    {
        private readonly INodeListManager _nodeListManager;
        private readonly ILogger<NodesController> _logger;

        public NodesController(INodeListManager nodeListManager, ILogger<NodesController> logger)
        {
            _nodeListManager = nodeListManager;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> GetAll()
        {
            //_nodeListManager.AddNode(Request.HttpContext.Connection.RemoteIpAddress);
            return _nodeListManager.GetNodes().Select(ip => ip.ToString());
        }
    }
}