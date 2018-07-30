namespace Hale.Core.Controllers
{
    using Hale.Core.Model.Interfaces;
    using Hale.Core.Services;
    using NLog;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Handles requests regarding results for a given node.
    /// </summary>
    [Route("api/v1/nodes")]
    public class NodesResultsController : ProtectedApiController
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly INodeResultsService nodeResultsService;

        public NodesResultsController()
            : this(new NodeResultsService())
        {
        }

        public NodesResultsController(INodeResultsService nodeResultsService)
        {
            this.nodeResultsService = nodeResultsService;
        }

        /// <summary>
        /// Lists the latest result for all functions connected to a node.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/results")]
        public ActionResult List(int id)
        {
            var nodeResults = this.nodeResultsService.List(id);
            return this.Ok(nodeResults);
        }

        /// <summary>
        /// Get result history for a given function.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="functionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/results/{functionId}")]
        public ActionResult Get(int nodeId, int functionId)
        {
            return Ok();
        }
    }
}
