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

        [Authorize]
        [Route()]
        [ResponseType(typeof(List<Host>))]
        [AcceptVerbs("GET")]
        public IHttpActionResult List()
        {
            var hostList = _db.Hosts.Include("HostDetails").ToList();
            return Ok(hostList);
        }

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
