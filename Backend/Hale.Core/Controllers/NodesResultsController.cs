namespace Hale.Core.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Hale.Core.Model.Interfaces;
    using Hale.Core.Services;
    using NLog;

    /// <summary>
    /// Handles requests regarding results for a given node.
    /// </summary>
    [RoutePrefix("api/v1/nodes")]
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
        public IHttpActionResult List(int id)
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
        public IHttpActionResult Get(int nodeId, int functionId)
        {
            return this.Ok();
        }
    }
}
