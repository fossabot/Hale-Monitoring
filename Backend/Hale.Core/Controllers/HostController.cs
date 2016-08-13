using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Hale.Core.Contexts;
using Hale.Core.Models.Nodes;
using NLog;
using System.Linq;

namespace Hale.Core.Controllers
{
    /// <summary>
    /// API for handling Host entries and related data.
    /// </summary>
    [RoutePrefix("api/v1/hosts")]
    public class HostController : ApiController
    {
        private readonly Logger _log;
        private readonly HaleDBContext _db = new HaleDBContext();

        internal HostController()
        {
            _log = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// List host entities. (Auth)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route()]
        [ResponseType(typeof(List<Host>))]
        [AcceptVerbs("GET")]
        public IHttpActionResult List()
        {
            var hostList = _db.Hosts.Include("HostDetaiks").ToList();
            return Ok(hostList);
        }

        /// <summary>
        /// Get information on a specific host. (Auth)
        /// </summary>
        /// <param name="id">Host ID of the host in question.</param>
        /// <returns></returns>
        [Authorize]
        [Route("{id}")]
        [ResponseType(typeof (Host))]
        [AcceptVerbs("GET")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var host = _db.Hosts.Include("HostDetails").FirstOrDefault( h => h.Id == id );
                if (host == null)
                    return NotFound();
                return Ok(host);
            }

            catch(Exception x)
            {
                return InternalServerError(x);
            }
        }

    }
}
