using Hale.Core.Contexts;
using Hale.Core.Models.Modules;
using NLog;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;

namespace Hale.Core.Controllers
{
    /// <summary>
    /// Handles requests regarding results for a given node.
    /// </summary>
    [RoutePrefix("api/v1/nodes")]
    public class NodesResultsController: ProtectedApiController
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Lists the latest result for all functions connected to a node.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/results")]
        public IHttpActionResult List(int id)
        {
            _db.Database.Log = s => System.Diagnostics.Debug.Write(s);
            var results = _db.Results
                .GroupBy(x => new { x.HostId, x.FunctionId, x.Target})
                .Select(x => x.OrderByDescending(r => r.ExecutionTime).FirstOrDefault())
                .Select(x => new NodeResultDTO {
                    NodeId = x.HostId,
                    FunctionId = x.FunctionId,
                    FriendlyName = _db.Modules.FirstOrDefault(r => r.Id == x.ModuleId).Identifier + ": " + _db.Functions.FirstOrDefault(r => r.Id == x.FunctionId).Name,
                    Target = x.Target,
                })
                .ToList();


            return Ok(results);
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
            return Ok();
        }
    }
}
