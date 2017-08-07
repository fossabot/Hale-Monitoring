using System;
using System.Collections.Generic;
using System.Text;
using Hale.Lib.Config;
using Hale.Lib.Modules.Actions;
using Hale.Lib.Modules.Checks;
using Hale.Lib.Modules.Info;
using YamlDotNet.Serialization;

using ModuleFunctionType = Hale.Lib.Modules.ModuleFunctionType;
using YamlDotNet.Serialization.NamingConventions;
using System.IO;
using Hale.Core.Data.Entities;

namespace Hale.Core.Utils
{
    class ConfigSerializer
    {
        internal static string SerializeAgentConfig(AgentConfigSet agentConfig)
        {
            var config = AgentConfig.Empty;

            foreach (var task in agentConfig.Tasks)
            {
                config.Tasks.Add(task.Name, new AgentConfigTask()
                {
                    Enabled = task.Enabled,
                    Interval = task.Interval,
                    Startup = task.Startup
                });
            }

            // TODO: Use proper keys or remove keys
            // Should we just skip the function identifiers/names? 
            // They are only used as keys in the config anyway -NM 2017-08-05
            var dummyNameInt = 0;

            foreach(var func in agentConfig.Functions)
            {
                var dummyName = "func" + dummyNameInt++;
                switch (func.Type) {
                    case ModuleFunctionType.Action:

                        var actionSettings = new ActionSettings()
                        {
                            Function = func.Function,
                            Enabled = func.Enabled,
                            Interval = func.Interval,
                            Module = func.Module.Identifier,
                            Startup = func.Startup,

                        };

                        foreach(var target in func.Targets)
                        {
                            var targetSettings = func.FunctionSettings.GetTargetDictionary(target);
                            actionSettings.TargetSettings.Add(target, targetSettings);
                        }

                        config.Actions.Add(dummyName, actionSettings);
                        break;

                    case ModuleFunctionType.Check:
                        var checkSettings = new CheckSettings()
                        {
                            Function = func.Function,
                            Enabled = func.Enabled,
                            Interval = func.Interval,
                            Module = func.Module.Identifier,
                            Startup = func.Startup,

                        };

                        foreach (var fs in func.FunctionSettings)
                        {
                            if(!checkSettings.TargetSettings.ContainsKey(fs.Target))
                            {
                                checkSettings.TargetSettings.Add(fs.Target, new Dictionary<string, string>());
                            }

                            checkSettings.TargetSettings[fs.Target].Add(fs.Key, fs.Value);
                        }

                        config.Checks.Add(dummyName, checkSettings);
                        break;

                    case ModuleFunctionType.Info:
                        var infoSettings = new InfoSettings()
                        {
                            Function = func.Function,
                            Enabled = func.Enabled,
                            Interval = func.Interval,
                            Module = func.Module.Identifier,
                            Startup = func.Startup,

                        };

                        foreach (var target in func.Targets)
                        {
                            var targetSettings = func.FunctionSettings.GetTargetDictionary(target);
                            infoSettings.TargetSettings.Add(target, targetSettings);
                        }

                        config.Info.Add(dummyName, infoSettings);
                        break;
                }
            }

            var ys = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            return ys.Serialize(config);


        }
    }

    static class ListExtensions
    {
        public static Dictionary<string, string> GetTargetDictionary(this List<AgentConfigSetFunctionSettings> list, string target)
        {
            var dict = new Dictionary<string, string>();
            
            foreach(var fs in list)
            {
                if(fs.Target == target)
                {
                    dict.Add(fs.Key, fs.Value);
                }
            }

            return dict;
        }
    }
}
