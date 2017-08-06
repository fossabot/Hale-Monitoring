using Hale.Core.Contexts;
using Hale.Core.Models.Agent;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using Hale.Core.Utils;
using Hale.Lib.Modules;
using Hale.Core.Models;
using System.Linq.Expressions;
using System;
using Hale.Core.Models.Users;

namespace Hale.Core.Controllers
{
    [RoutePrefix("api/v1/configs")]
    public class ConfigController : ProtectedApiController
    {

        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private Expression<Func<Account, UserSummaryDTO>> CreateUserSummaryDTO = account => new UserSummaryDTO
        {
            Id = account.Id,
            FullName = account.FullName,
            Email = account.Email,
        };

        [Route()]
        [ResponseType(typeof(List<AgentConfigSet>))]
        [AcceptVerbs("GET")]
        public IHttpActionResult List()
        {



            var configList = _db.AgentConfigs
                .Include( c => c.Functions )
                .Include( c => c.Tasks )
                .Select(c => new ConfigSummaryDTO
                {
                    Id = c.Id,
                    Identifier = c.Identifier,
                    CheckCount = c.Functions.Where(x => x.Type == ModuleFunctionType.Check).Count(),
                    ActionCount = c.Functions.Where(x => x.Type == ModuleFunctionType.Action).Count(),
                    InfoCount = c.Functions.Where(x => x.Type == ModuleFunctionType.Info).Count(),
                    Modified = c.Modified,
                    ModifiedBy = _db.Accounts.Where(a => a.Id == c.ModifiedBy).Select(CreateUserSummaryDTO).FirstOrDefault(),
                    Created = c.Created,
                    CreatedBy = _db.Accounts.Where(a => a.Id == c.CreatedBy).Select(CreateUserSummaryDTO).FirstOrDefault(),
                }).ToList();
            return Ok(configList);
        }

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

        [HttpPost, Route("{id}")]
        public IHttpActionResult Update(int id, [FromBody]ConfigSourceDTO configSource)
        {
            _log.Info($"Got save for #{id}:\n{configSource.Body}");

            return Ok();
        }

    }
}
