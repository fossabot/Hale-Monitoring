namespace Hale.Core.Utils
{
    using System.Collections.Generic;
    using Hale.Core.Data.Entities.Agent;
    using Hale.Lib.Config;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Actions;
    using Hale.Lib.Modules.Checks;
    using Hale.Lib.Modules.Info;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;
    using ModuleFunctionType = Hale.Lib.Modules.ModuleFunctionType;

    internal class ConfigSerializer
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
                    Startup = task.Startup,
                });
            }

            foreach (var func in agentConfig.Functions)
            {
                void SetFunctionSettings(ModuleSettingsBase ms)
                {
                    ms.Function = func.Function;
                    ms.Enabled = func.Enabled;
                    ms.Interval = func.Interval;
                    ms.Module = new VersionedIdentifier(func.Module.Identifier, func.Module.Version);
                    ms.Startup = func.Startup;

                    foreach (var fs in func.FunctionSettings)
                    {
                        if (!ms.TargetSettings.ContainsKey(fs.Target))
                        {
                            ms.TargetSettings.Add(fs.Target, new Dictionary<string, string>());
                        }

                        ms.TargetSettings[fs.Target].Add(fs.Key, fs.Value);
                    }
                }

                CheckAction SerializeCheckAction(AgentConfigSetCheckAction ca)
                    => ca == null ? null : new CheckAction()
                    {
                        Action = ca.Action,
                        Module = ca.Module,
                        Target = ca.Target
                    };

                switch (func.Type)
                {
                    case ModuleFunctionType.Check:
                        var checkSettings = new CheckSettings();
                        SetFunctionSettings(checkSettings);

                        checkSettings.Actions = new CheckActions()
                        {
                            Critical = SerializeCheckAction(func.CriticalAction),
                            Warning = SerializeCheckAction(func.WarningAction),
                        };

                        checkSettings.Thresholds = new CheckThresholds()
                        {
                            Critical = func.CriticalThreshold,
                            Warning = func.WarningThreshold,
                        };

                        config.Checks.Add(checkSettings);
                        break;

                    case ModuleFunctionType.Action:
                        var actionSettings = new ActionSettings();
                        SetFunctionSettings(actionSettings);
                        config.Actions.Add(actionSettings);
                        break;

                    case ModuleFunctionType.Info:
                        var infoSettings = new InfoSettings();
                        SetFunctionSettings(infoSettings);
                        config.Info.Add(infoSettings);
                        break;
                }
            }

            var ys = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            return ys.Serialize(config);
        }
    }
}
