using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Globalization;
using Hale.Lib.Modules.Checks;
using Hale.Lib.Modules.Info;
using Hale.Lib.Modules.Actions;
using System.Collections;
using Hale.Lib.Modules;

namespace Hale.Lib.Config
{
    public abstract class AgentConfigBase
    {
        public Dictionary<string, AgentConfigModule> Modules { get; set; }
        public Dictionary<string, AgentConfigTask> Tasks { get; set; } = new Dictionary<string, AgentConfigTask>();
    }

    class AgentConfigRaw: AgentConfigBase
    {
        public Dictionary<string, Dictionary<string, object>> Checks { get; set; }
            = new Dictionary<string, Dictionary<string, object>>();

        public Dictionary<string, Dictionary<string, object>> Info { get; set; }
            = new Dictionary<string, Dictionary<string, object>>();

        public Dictionary<string, Dictionary<string, object>> Actions { get; set; }
            = new Dictionary<string, Dictionary<string, object>>();
    }

    public class AgentConfig: AgentConfigBase
    {
        public Dictionary<string, CheckSettings> Checks { get; set; }

        public Dictionary<string, InfoSettings> Info { get; set; }

        public Dictionary<string, ActionSettings> Actions { get; set; }




        /// <summary>
        /// Returns a new empty instance of AgentConfig with all fields initialized
        /// </summary>
        public static AgentConfig Empty => new AgentConfig().InitializeAgentConfigLists();

        static readonly NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };

        public static AgentConfig LoadFromFile(string file)
        {
            AgentConfigRaw acr;

            using (StreamReader sr = File.OpenText(file))
            {
                Deserializer ds = new Deserializer(namingConvention: new CamelCaseNamingConvention());
                acr = ds.Deserialize<AgentConfigRaw>(sr);
            }

            CheckAgentConfigForNull(acr);

            var ac = AgentConfig.Empty;
            ac = SetDefaultTaskValues(ac);

            if (acr.Checks != null)
                ac.LoadSerializedCheck(acr);

            if (acr.Info != null)
                ac.LoadSerializedInfo(acr);

            if (acr.Actions != null)
                ac.LoadSerializedActions(acr);

            return ac;
        }

        private AgentConfig InitializeAgentConfigLists()
        {
            Checks = new Dictionary<string, CheckSettings>();
            Info = new Dictionary<string, InfoSettings>();
            Actions = new Dictionary<string, ActionSettings>();
            return this;
        }

        private static AgentConfig SetDefaultTaskValues(AgentConfig ac)
        {
            if (ac == null)
                ac = new AgentConfig();

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

        private void LoadSerializedActions(AgentConfigRaw acr)
        {
            foreach (var action in acr.Actions)
            {
                var actionProperties = GetProperties(action.Value);
                var actionSettings = new ActionSettings(actionProperties);
                actionSettings.TargetSettings = new Dictionary<string, Dictionary<string, string>>();
                var targetSettings = new Dictionary<string, string>();
                foreach (var target in GetTargets(action.Value))
                    actionSettings.TargetSettings.Add(target.Key, targetSettings);
                actionSettings.ParseRaw();

                Actions.Add(action.Key, actionSettings);
            }
        }

        private void LoadSerializedInfo(AgentConfigRaw acr)
        {
            foreach (var info in acr.Info)
            {
                var infoProperties = GetProperties(info.Value);
                var infoSettings = new InfoSettings(infoProperties);
                infoSettings.TargetSettings = new Dictionary<string, Dictionary<string, string>>();
                var targetSettings = new Dictionary<string, string>();
                foreach (var target in GetTargets(info.Value))
                    infoSettings.TargetSettings.Add(target.Key, targetSettings);
                infoSettings.ParseRaw();

                Info.Add(info.Key, infoSettings);
            }
        }

        private void LoadSerializedCheck(AgentConfigRaw acr)
        {
            foreach (var check in acr.Checks)
            {
                var checkProperties = GetProperties(check.Value);
                var checkSettings = new CheckSettings(checkProperties);
                checkSettings.TargetSettings = new Dictionary<string, Dictionary<string, string>>();
                checkSettings.Actions = GetActions(check.Value);
                checkSettings.Thresholds = GetThresholds(check.Value);
                var targetSettings = new Dictionary<string, string>();
                foreach (var target in GetTargets(check.Value))
                    checkSettings.TargetSettings.Add(target.Key, targetSettings);
                checkSettings.ParseRaw();

                Checks.Add(check.Key, checkSettings);
            }
        }

        private static void CheckAgentConfigForNull(AgentConfigRaw acr)
        {
            if (acr == null)
            {
                throw new Exception("Could not deserialize config.yaml. Make sure the syntax is correct.");
            }
        }

        private static CheckThresholds GetThresholds(Dictionary<string, object> input)
        {
            if (input.ContainsKey("thresholds"))
            {
                var td = (IDictionary)input["thresholds"];
                return new CheckThresholds()
                {
                    Critical = Single.Parse(td["critical"].ToString(), nfi),
                    Warning = Single.Parse(td["warning"].ToString(), nfi)
                };
            }
            else
            {
                return new CheckThresholds() { Warning = 0.5F, Critical = 1.0F };
            }
        }

        private static CheckActions GetActions(Dictionary<string, object> input)
        {
            return (input.ContainsKey("actions") ?input["actions"] as CheckActions : new CheckActions());
        }

        static Dictionary<string, Dictionary<string, string>> GetTargets(Dictionary<string, object> input)
        {
            var targets = new Dictionary<string, Dictionary<string, string>>();
            if (input.ContainsKey("targets")) {
                foreach(DictionaryEntry kvpTarget in (IDictionary)input["targets"])
                {
                    Dictionary<string, string> targetSettings = new Dictionary<string, string>();
                    if(kvpTarget.Value != null)
                    {
                        foreach(DictionaryEntry kvpSettings in (IDictionary)kvpTarget.Value)
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

        static Dictionary<string, string> GetProperties(Dictionary<string, object> input)
        {
            string[] ignoreKeys = new []{ "targets", "actions", "thresholds" };
            return input.Where(item => !ignoreKeys.Contains(item.Key))
                .ToDictionary(item => item.Key, item => item.Value as string);
        }
    }

    public class AgentConfigTask
    {
        [YamlIgnore]
        public int Id { get; set; }

        public bool Enabled { get; set; } = true;
        public TimeSpan Interval { get; set; }
        public bool Startup { get; set; } = false;
    }

    public class AgentConfigModule
    {
        public string Dll { get; set; }
    }
}
