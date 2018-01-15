using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ProtoChain.Network;

namespace ProtoChain.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/Nodes")]
    public class NodesController : Controller
    {
        private readonly INodeListManager _nodeListManager;

        public NodesController(INodeListManager nodeListManager)
        {
            _nodeListManager = nodeListManager;
        }

        [HttpGet]
        public IEnumerable<string> GetAll()
        {
            _nodeListManager.AddNode(Request.HttpContext.Connection.RemoteIpAddress);
            return _nodeListManager.GetNodes().Select(ip => ip.ToString());
        }
    }
}