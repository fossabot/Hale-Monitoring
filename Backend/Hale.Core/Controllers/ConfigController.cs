namespace Hale.Core.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Hale.Core.Data.Entities.Agent;
    using Hale.Core.Model.Interfaces;
    using Hale.Core.Models;
    using Hale.Core.Services;
    using NLog;

    [RoutePrefix("api/v1/configs")]
    public class ConfigController : ProtectedApiController
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly IConfigService configService;

        public ConfigController()
            : this(new ConfigService())
        {
        }

        public ConfigController(IConfigService configService)
        {
            this.configService = configService;
        }

        [Route]
        [ResponseType(typeof(List<AgentConfigSet>))]
        [AcceptVerbs("GET")]
        public IHttpActionResult List()
        {
            var configList = this.configService.List();
            return this.Ok(configList);
        }

        [Route("{id}")]
        [ResponseType(typeof(string))]
        [AcceptVerbs("GET")]
        public IHttpActionResult Get(int id)
        {
            var config = this.configService.GetConfigById(id);
            return this.Ok(config);
        }

        [HttpPost]
        [Route("{id}")]
        public IHttpActionResult Update(int id, [FromBody]ConfigSourceDTO configSource)
        {
            this.log.Info($"Got save for #{id}!");
            var changes = this.configService.SaveSerialized(id, configSource.Body, this.CurrentUsername);
            this.log.Info($"Wrote {changes} change(s).");
            return this.Ok();
        }
    }
}
