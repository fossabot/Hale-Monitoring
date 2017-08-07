using Hale.Core.Data.Entities;
using Hale.Core.Model.Interfaces;
using Hale.Core.Models;
using Hale.Core.Utils;
using Hale.Lib.Modules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Hale.Core.Services
{
    public class ConfigService: HaleBaseService, IConfigService
    {
        private Expression<Func<Account, UserSummaryDTO>> CreateUserSummaryDTO = account => new UserSummaryDTO
        {
            Id = account.Id,
            FullName = account.FullName,
            Email = account.Email,
        };

        public IList<ConfigSummaryDTO> List()
        {
            var configs = _db.AgentConfigs
                .Include(c => c.Functions)
                .Include(c => c.Tasks)
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
            return configs;
        }

        public string GetConfigById(int id)
        {
            var agentConfig = _db.AgentConfigs
                .Include(c => c.Functions.Select(f => f.Module))
                .Include(c => c.Functions.Select(f => f.FunctionSettings))
                .Include(c => c.Tasks)
                .FirstOrDefault(c => c.Id == id);

            var serializedConfig = ConfigSerializer.SerializeAgentConfig(agentConfig);
            return serializedConfig;
        }
    }
}
