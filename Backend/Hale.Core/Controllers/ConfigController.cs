using Hale.Core.Contexts;
using Hale.Core.Models.Agent;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using Hale.Core.Utils;

namespace Hale.Core.Controllers
{
    [RoutePrefix("api/v1/configs")]
    public class ConfigController : ApiController
    {

        #region Constructors and declarations
        private readonly ILogger _log;
        private readonly HaleDBContext _db;

        internal ConfigController() : this(new HaleDBContext()) { }
        internal ConfigController(HaleDBContext context)
        {
            _db = context;
            _log = LogManager.GetCurrentClassLogger();
        }
        #endregion

        //[Authorize]
        [Route()]
        [ResponseType(typeof(List<AgentConfigSet>))]
        [AcceptVerbs("GET")]
        public IHttpActionResult List()
        {
            var configList = _db.AgentConfigs
                .Include( c => c.Functions.Select( f => f.Module ) )
                .Include( c => c.Tasks )
                .ToList();
            return Ok(configList);
        }

        //[Authorize]
        [Route("{id}")]
        [ResponseType(typeof(List<AgentConfigSet>))]
        [AcceptVerbs("GET")]
        public IHttpActionResult Get(int id)
        {
            var agentConfig = _db.AgentConfigs
                .Include(c => c.Functions.Select(f => f.Module))
                .Include(c => c.Functions.Select(f => f.FunctionSettings))
                .Include(c => c.Tasks)
                .FirstOrDefault(c => c.Id == id);


            var serializedConfig = ConfigSerializer.SerializeAgentConfig(agentConfig);

            return Ok(serializedConfig);
        }

    }
}
