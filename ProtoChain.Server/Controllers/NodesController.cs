using System.Collections.Generic;
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
            return _nodeListManager.GetNodeList();
        }
    }
}