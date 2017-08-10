using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using NLog;
using Hale.Core.Models.Messages;
using Hale.Core.Data.Entities;
using Hale.Core.Model.Interfaces;
using Hale.Core.Services;
using Hale.Core.Model.Models;
using Hale.Core.Data.Entities.Nodes;

namespace Hale.Core.Controllers
{
    [RoutePrefix("api/v1/nodes")]
    public class NodesController : ProtectedApiController
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly INodesService _nodesService;

        public NodesController(): this(new NodesService()) { }
        public NodesController(INodesService nodesService)
        {
            _nodesService = nodesService;
        }

        /// <summary>
        /// Get a list of summaries for all nodes
        /// </summary>
        /// <returns></returns>
        [Route()]
        [ResponseType(typeof(List<Node>))]
        [AcceptVerbs("GET")]
        public IHttpActionResult List()
        {

            var hostList = _nodesService.List();
            return Ok(hostList);
        }

        /// <summary>
        /// Get Node Details by Node ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(Node))]
        [AcceptVerbs("GET")]
        public IHttpActionResult Get(int id)
        {
            var node = _nodesService.GetNodeById(id);
            if (node == null)
                return NotFound();

            return Ok(node);
        }

        /// <summary>
        /// Used to update the host from the webapi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hostToSave"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPost]
        public IHttpActionResult Update(int id, [FromBody] NodeDTO hostToSave)
        {
            _nodesService.Update(id, hostToSave, _currentUsername);
            return Ok();
        }

        /// <summary>
        /// Gets all comments for the supplied node id.
        /// </summary>
        /// <param name="id">The id of the node</param>
        /// <returns></returns>
        [Route("{id}/comments")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetComments(int id)
        {
            var comments = _nodesService.GetComments(id);
            return Ok(comments);
        }

        /// <summary>
        /// Create a new comment on a node
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newComment"></param>
        /// <returns></returns>
        [Authorize]
        [Route("{id}/comments")]
        [AcceptVerbs("POST")]
        public IHttpActionResult SaveComment(int id, [FromBody] NewCommentDTO newComment)
        {
            _nodesService.SaveComment(id, newComment, _currentUsername);
            return Ok();
        }

        /// <summary>
        /// Delete a comment on a node
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [Authorize]
        [Route("{id}/comments/{commentId}")]
        [AcceptVerbs("DELETE")]
        public IHttpActionResult DeleteComment(int id, int commentId)
        {
            _nodesService.DeleteComment(id, commentId);
            return Ok();
        }
    }
}
