namespace Hale.Core.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Hale.Core.Data.Entities.Nodes;
    using Hale.Core.Model.Interfaces;
    using Hale.Core.Model.Models;
    using Hale.Core.Models.Messages;
    using Hale.Core.Services;
    using NLog;

    [RoutePrefix("api/v1/nodes")]
    public class NodesController : ProtectedApiController
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly INodesService nodesService;

        public NodesController()
            : this(new NodesService())
        {
        }

        public NodesController(INodesService nodesService)
        {
            this.nodesService = nodesService;
        }

        /// <summary>
        /// Get a list of summaries for all nodes
        /// </summary>
        /// <returns></returns>
        [Route]
        [ResponseType(typeof(List<Node>))]
        [AcceptVerbs("GET")]
        public IHttpActionResult List()
        {
            var hostList = this.nodesService.List();
            return this.Ok(hostList);
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
            var node = this.nodesService.GetNodeById(id);
            if (node == null)
            {
                return this.NotFound();
            }

            return this.Ok(node);
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
            this.nodesService.Update(id, hostToSave, this.CurrentUsername);
            return this.Ok();
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
            var comments = this.nodesService.GetComments(id);
            return this.Ok(comments);
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
            this.nodesService.SaveComment(id, newComment, this.CurrentUsername);
            return this.Ok();
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
            this.nodesService.DeleteComment(id, commentId);
            return this.Ok();
        }
    }
}
