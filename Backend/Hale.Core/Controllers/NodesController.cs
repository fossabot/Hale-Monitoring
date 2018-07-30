namespace Hale.Core.Controllers
{
    using System.Collections.Generic;
    using Hale.Core.Data.Entities;
    using Hale.Core.Data.Entities.Nodes;
    using Hale.Core.Model.Interfaces;
    using Hale.Core.Model.Models;
    using Hale.Core.Models.Messages;
    using Hale.Core.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NLog;

    [Route("api/v1/nodes")]
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
        [HttpGet]
        public IList<NodeSummaryDTO> List()
            => this.nodesService.List();

        /// <summary>
        /// Get Node Details by Node ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [AcceptVerbs("GET")]
        public IActionResult Get(int id)
        {
            var node = this.nodesService.GetNodeById(id);
            if (node == null)
            {
                return NotFound();
            }

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
        public IActionResult Update(int id, [FromBody] NodeDTO hostToSave)
        {
            this.nodesService.Update(id, hostToSave, this.CurrentUsername);
            return Ok();
        }

        /// <summary>
        /// Gets all comments for the supplied node id.
        /// </summary>
        /// <param name="id">The id of the node</param>
        /// <returns></returns>
        [Route("{id}/comments")]
        [AcceptVerbs("GET")]
        public IActionResult GetComments(int id)
        {
            var comments = this.nodesService.GetComments(id);
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
        public IActionResult SaveComment(int id, [FromBody] NewCommentDTO newComment)
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
        public IActionResult DeleteComment(int id, int commentId)
        {
            this.nodesService.DeleteComment(id, commentId);
            return this.Ok();
        }
    }
}
