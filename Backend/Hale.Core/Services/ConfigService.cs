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

        public int SaveSerialized(int id, string serialized, string currentUsername)
        {
            var newConfig = Lib.Config.AgentConfig.LoadFromString(serialized);

            var configSet = _db.AgentConfigs.Where(x => x.Id == id)
                .Include(cs => cs.Tasks)
                .Include(cs => cs.Functions)
                .FirstOrDefault();

            var currentUser = _db.Accounts.First(x => x.UserName == currentUsername);

            configSet.Modified = DateTime.Now;
            configSet.ModifiedBy = currentUser.Id;

            foreach(var task in newConfig.Tasks)
            {
                var act = configSet.Tasks?.FirstOrDefault(t => t.Name == task.Key) 
                    ?? new AgentConfigSetTask() { Name = task.Key };
                act.Enabled = task.Value.Enabled;
                act.Interval = task.Value.Interval;
                act.Startup = task.Value.Startup;
            }

            AgentConfigSetFuncSettings UpdateFunction(ModuleSettingsBase ms, IEnumerable<AgentConfigSetFuncSettings> fs)
            {
                AgentConfigSetFuncSettings func;
                var candidates = fs.Where(
                    c => c.Module != null && 
                    c.Module.Identifier == ms.Module.Identifier &&
                    c.Module.Version == ms.Module.Version &&
                    c.Function == ms.Function);

                if (candidates.Count() == 1)
                {
                    // Only a single row matches our criteria and we can reuse it
                    func = candidates.First();
                }
                else
                {
                    // Many or no rows matches our critera, we cannot reuse them.
                    // TODO: Handle this somehow? -NM 2017-08-07

                    // Clean up old references
                    // NOTE: Does not work as is as it will conflict with new rows -NM 2017-08-07
                    //_db.AgentConfigSetFuncSettings.RemoveRange(candidates);

                    // Create new entity
                    func = new AgentConfigSetFuncSettings()
                    {
                        Function = ms.Function,
                        Type = ModuleFunctionType.Check,
                        Module = _db.Modules.FirstOrDefault(
                            m => m.Identifier == ms.Module.Identifier && 
                            m.Major == ms.Module.Version.Major &&
                            m.Minor == ms.Module.Version.Minor &&
                            m.Revision == ms.Module.Version.Revision

                            )

                            // FIXME: We probably shouldn't allow saving a configuration file with an unknown module identifier or version
                            ?? new Data.Entities.Module
                            {
                                Identifier = ms.Module.Identifier,
                                Version = ms.Module.Version
                            },
                    };
                }


                func.Enabled = ms.Enabled;
                func.Interval = ms.Interval;
                func.Startup = ms.Startup;

                // TODO: Serialize function target settings
                //func.FunctionSettings

                return func;
            }

            var updatedFuncs = new List<AgentConfigSetFuncSettings>();

            var checks = configSet?.Functions.Where(f => f.Type == ModuleFunctionType.Check);
            foreach(var check in newConfig.Checks)
            {
                var func = UpdateFunction(check, checks);

                AgentConfigSetCheckAction SetCheckAction(Lib.Modules.Checks.CheckAction ca)
                { 
                    return ca == null ? null : _db.AgentConfigSetCheckActions.FirstOrDefault(
                    a => a.Module == ca.Module &&
                    a.Action == ca.Action) ?? new AgentConfigSetCheckAction()
                    {
                        Action = ca.Action,
                        Module = ca.Module,
                        Target = ca.Target
                    };
                }

                func.CriticalAction = SetCheckAction(check?.Actions?.Critical);
                func.CriticalThreshold = check.Thresholds.Critical;

                func.WarningAction = SetCheckAction(check?.Actions?.Warning);
                func.WarningThreshold = check.Thresholds.Warning;

                updatedFuncs.Add(func);

            }

            var actions = configSet.Functions.Where(f => f.Type == ModuleFunctionType.Action);
            foreach (var action in newConfig.Actions)
            {
                var func = UpdateFunction(action, actions);
                updatedFuncs.Add(func);
            }

            var infos = configSet.Functions.Where(f => f.Type == ModuleFunctionType.Info);
            foreach (var info in newConfig.Info)
            {
                var func = UpdateFunction(info, infos);
                updatedFuncs.Add(func);
            }

            configSet.Functions = updatedFuncs;

            return _db.SaveChanges();
        }
    }
}
