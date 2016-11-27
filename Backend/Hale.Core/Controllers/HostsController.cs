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

namespace Hale.Core.Controllers
{
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    [RoutePrefix("api/v1/hosts")]
    public class HostsController : ApiController
    {
        #region Constructors and declarations
        private readonly Logger _log;
        private readonly HaleDBContext _db;

        internal HostsController() : this(new HaleDBContext()) { }
        internal HostsController(HaleDBContext context)
        {
            _db = context;
            _log = LogManager.GetCurrentClassLogger();
        }
        #endregion

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <returns></returns>
        [Authorize]
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
        [Authorize]
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


        private string _currentUsername {
            get {
                return Request.GetOwinContext().Authentication.User.Identities.First().Claims
                    .First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }
        }

        /// <summary>
        /// Used to update the host from the webapi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hostToSave"></param>
        /// <returns></returns>
        [Authorize]
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
    }
}
