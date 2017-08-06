using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Hale.Core.Contexts;
using Hale.Core.Models.Nodes;
using NLog;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using Hale.Core.Models.Messages;

namespace Hale.Core.Controllers
{
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    [RoutePrefix("api/v1/nodes")]
    public class NodesController : ProtectedApiController
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        #region Endpoints for Node Data

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <returns></returns>
        [Route()]
        [ResponseType(typeof(List<Host>))]
        [AcceptVerbs("GET")]
        public IHttpActionResult List()
        {
            var hostList = _db.Hosts.Include("HostDetails").ToList();
            return Ok(hostList);
        }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(Host))]
        [AcceptVerbs("GET")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var host = _db.Hosts.Include("HostDetails").FirstOrDefault(h => h.Id == id);
                if (host == null)
                    return NotFound();
                return Ok(host);
            }

            catch (Exception x)
            {
                return InternalServerError(x);
            }
        }




        /// <summary>
        /// Used to update the host from the webapi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hostToSave"></param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(Host))]
        [HttpPost]
        public IHttpActionResult Update(int id, [FromBody] Host hostToSave)
        {
            try
            {
                var user = _db.Accounts.First(x => x.UserName == _currentUsername);
                var host = _db.Hosts.FirstOrDefault(x => x.Id == id);
                
                if (host == null)
                    return NotFound();

                host.FriendlyName = hostToSave.FriendlyName;
                host.Domain = hostToSave.Domain;
                host.ModifiedBy = user.Id;
                host.ConfiguredBy = user.Id;

                host.Configured = !hostToSave.Blocked;
                host.Blocked = hostToSave.Blocked;

                _db.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            
        }
        #endregion

        #region Endpoints for Node Comments
        /// <summary>
        /// Gets all comments for the supplied node id.
        /// </summary>
        /// <param name="id">The id of the node</param>
        /// <returns></returns>
        [Route("{id}/comments")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetComments(int id)
        {
            try
            {
                var comments = _db.HostComments.Include("User").Where((x => x.Node.Id == id)).ToList();
                return Ok(comments);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
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
        public IHttpActionResult SaveComment(int id, [FromBody] NewComment newComment)
        {
            var user = _db.Accounts.First(x => x.UserName == _currentUsername);
            var node = _db.Hosts.FirstOrDefault(x => x.Id == id);
            var comment = new HostComment()
            {
                Text = newComment.Text,
                Timestamp = DateTime.Now,
                User = user,
                Node = node
            };
            try
            {
                _db.HostComments.Add(comment);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
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
            try
            {
                var comment = _db.HostComments.First(x => x.Id == commentId);
                _db.HostComments.Remove(comment);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        #endregion

        private string _currentUsername {
            get {
                return Request.GetOwinContext().Authentication.User.Identities.First().Claims
                    .First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }
        }
    }
}
