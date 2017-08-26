namespace Hale.Lib.Config
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Hale.Lib.Modules.Actions;
    using Hale.Lib.Modules.Checks;
    using Hale.Lib.Modules.Info;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    public class AgentConfig : AgentConfigBase
    {
        private static readonly NumberFormatInfo Nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };

        /// <summary>
        /// Returns a new empty instance of AgentConfig with all fields initialized
        /// </summary>
        public static AgentConfig Empty
            => new AgentConfig().InitializeAgentConfigLists();

        public List<CheckSettings> Checks { get; set; }

        public List<InfoSettings> Info { get; set; }

        public List<ActionSettings> Actions { get; set; }

        private static Deserializer Deserializer => new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .IgnoreUnmatchedProperties()
                .Build();

        public static Dictionary<string, Dictionary<string, string>> GetTargets(Dictionary<string, object> input)
        {
            var targets = new Dictionary<string, Dictionary<string, string>>();
            if (input.ContainsKey("targets"))
            {
                foreach (DictionaryEntry kvpTarget in (IDictionary)input["targets"])
                {
                    Dictionary<string, string> targetSettings = new Dictionary<string, string>();
                    if (kvpTarget.Value != null)
                    {
                        foreach (DictionaryEntry kvpSettings in (IDictionary)kvpTarget.Value)
                        {
                            targetSettings.Add(kvpSettings.Key.ToString(), kvpSettings.Value.ToString());
                        }
                    }

                    targets.Add(kvpTarget.Key.ToString(), targetSettings);
                }
            }

            if (targets.Count == 0)
            {
                targets.Add("default", new Dictionary<string, string>());
            }

            return targets;
        }

        internal static AgentConfig Load(AgentConfigRaw acr)
        {
            CheckAgentConfigForNull(acr);

            var ac = AgentConfig.Empty;
            ac = SetDefaultTaskValues(ac);

            if (acr.Checks != null)
            {
                ac.LoadSerializedCheck(acr);
            }

            if (acr.Info != null)
            {
                ac.LoadSerializedInfo(acr);
            }

            if (acr.Actions != null)
            {
                ac.LoadSerializedActions(acr);
            }

            ac.Modules = acr.Modules;
            ac.Tasks = acr.Tasks;

            return ac;
        }

        private static CheckActions GetActions(Dictionary<string, object> input)
            => input.ContainsKey("actions")
            ? input["actions"] as CheckActions
            : new CheckActions();

        private static void CheckAgentConfigForNull(AgentConfigRaw acr)
        {
            if (acr == null)
            {
                throw new Exception("Could not deserialize config.yaml. Make sure the syntax is correct.");
            }
        }

        private static AgentConfig SetDefaultTaskValues(AgentConfig ac)
        {
            if (ac == null)
            {
                ac = new AgentConfig();
            }

            if (!ac.Tasks.ContainsKey("persistResults"))
            {
                ac.Tasks.Add("persistResults", new AgentConfigTask()
                {
                    Enabled = false,
                    Interval = new TimeSpan(0, 10, 0),
                    Startup = false
                });
            }

            if (!ac.Tasks.ContainsKey("uploadResults"))
            {
                ac.Tasks.Add("uploadResults", new AgentConfigTask()
                {
                    Enabled = true,
                    Interval = new TimeSpan(0, 30, 0),
                    Startup = true
                });
            }

            if (!ac.Tasks.ContainsKey("sendHeartbeat"))
            {
                ac.Tasks.Add("sendHeartbeat", new AgentConfigTask()
                {
                    Enabled = false,
                    Interval = new TimeSpan(0, 10, 0),
                    Startup = true
                });
            }

            return ac;
        }

        private static CheckThresholds GetThresholds(Dictionary<string, object> input)
        {
            var ct = new CheckThresholds() { Warning = 0.5F, Critical = 1.0F };
            if (input.ContainsKey("thresholds"))
            {
                var td = (IDictionary)input["thresholds"];

                if (td.Contains("critical"))
                {
                    ct.Critical = float.Parse(td["critical"].ToString(), Nfi);
                }

                if (td.Contains("warning"))
                {
                    ct.Warning = float.Parse(td["warning"].ToString(), Nfi);
                }
            }

            return ct;
        }

        private static Dictionary<string, string> GetProperties(Dictionary<string, object> input)
        {
            string[] ignoreKeys = new[] { "targets", "actions", "thresholds" };
            return input.Where(item => !ignoreKeys.Contains(item.Key))
                .ToDictionary(item => item.Key, item => item.Value as string);
        }

        private AgentConfig InitializeAgentConfigLists()
        {
            this.Checks = new List<CheckSettings>();
            this.Info = new List<InfoSettings>();
            this.Actions = new List<ActionSettings>();
            return this;
        }

        private void LoadSerializedActions(AgentConfigRaw acr)
        {
            foreach (var action in acr.Actions)
            {
                var actionProperties = GetProperties(action);
                var actionSettings = new ActionSettings(actionProperties);
                actionSettings.TargetSettings = new Dictionary<string, Dictionary<string, string>>();
                var targetSettings = new Dictionary<string, string>();
                foreach (var target in GetTargets(action))
                {
                    actionSettings.TargetSettings.Add(target.Key, targetSettings);
                }

                actionSettings.ParseRaw();

                this.Actions.Add(actionSettings);
            }
        }

        private void LoadSerializedInfo(AgentConfigRaw acr)
        {
            foreach (var info in acr.Info)
            {
                var infoProperties = GetProperties(info);
                var infoSettings = new InfoSettings(infoProperties);
                infoSettings.TargetSettings = new Dictionary<string, Dictionary<string, string>>();
                var targetSettings = new Dictionary<string, string>();
                foreach (var target in GetTargets(info))
                {
                    infoSettings.TargetSettings.Add(target.Key, targetSettings);
                }

                infoSettings.ParseRaw();

                this.Info.Add(infoSettings);
            }
        }

        private void LoadSerializedCheck(AgentConfigRaw acr)
        {
            foreach (var check in acr.Checks)
            {
                var checkProperties = GetProperties(check);
                var checkSettings = new CheckSettings(checkProperties);
                checkSettings.TargetSettings = new Dictionary<string, Dictionary<string, string>>();
                checkSettings.Actions = GetActions(check);
                checkSettings.Thresholds = GetThresholds(check);
                var targetSettings = new Dictionary<string, string>();
                foreach (var target in GetTargets(check))
                {
                    checkSettings.TargetSettings.Add(target.Key, targetSettings);
                }

                checkSettings.ParseRaw();

                this.Checks.Add(checkSettings);
            }
        }
    }
}
