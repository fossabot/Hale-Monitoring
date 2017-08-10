using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using Hale.Core.Utils;
using Hale.Lib.Modules;
using System.Linq.Expressions;
using System;
using Hale.Core.Models;
using Hale.Core.Model.Interfaces;
using Hale.Core.Services;
using Hale.Core.Data.Entities.Agent;

namespace Hale.Core.Controllers
{
    [RoutePrefix("api/v1/configs")]
    public class ConfigController : ProtectedApiController
    {

        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly IConfigService _configService;
        public ConfigController(): this(new ConfigService()) { }
        public ConfigController(IConfigService configService)
        {
            _configService = configService;
        }

        [Route()]
        [ResponseType(typeof(List<AgentConfigSet>))]
        [AcceptVerbs("GET")]
        public IHttpActionResult List()
        {
            var configList = _configService.List();
            return Ok(configList);
        }

        [Route("{id}")]
        [ResponseType(typeof(string))]
        [AcceptVerbs("GET")]
        public IHttpActionResult Get(int id)
        {
            var config = _configService.GetConfigById(id);
            return Ok(config);
        }

        [HttpPost, Route("{id}")]
        public IHttpActionResult Update(int id, [FromBody]ConfigSourceDTO configSource)
        {
            _log.Info($"Got save for #{id}!");
            var changes = _configService.SaveSerialized(id, configSource.Body, _currentUsername);
            _log.Info($"Wrote {changes} change(s).");
            return Ok();
        }

    }
}
