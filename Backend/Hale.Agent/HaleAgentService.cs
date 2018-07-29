namespace Hale.Agent
{
    using System;
    using System.IO;
    using System.Threading;
    using Hale.Agent.Communication;
    using Hale.Agent.Config;
    using Hale.Agent.Modules;
    using Hale.Agent.Scheduler;
    using Hale.Lib.Config;
    using Hale.Lib.Utilities;
    using NLog;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

#if NETCOREAPP2_1
    using Hale.Lib.Facades;
#else
    using System.ServiceProcess;
#endif

    public partial class HaleAgentService : ServiceBase
    {
        private ILogger log = LogManager.GetLogger("Service");

        private AgentConfig config;
        private NemesisController nemesis;
        private AgentScheduler scheduler;
        private IResultStorage resultStorage;
        private EnvironmentConfig env;

        public HaleAgentService()
        {
            this.InitializeComponent();
        }

#if DEBUG
        internal void StartDebug()
        {
            this.OnStart(new string[0]);
        }
#endif

        protected override void OnStart(string[] args)
        {
            this.env = new EnvironmentConfig();
            this.env.DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Hale", "Agent");
            ServiceProvider.SetService(this.env);

            this.InitializeNemesis();

            this.UpdateConfiguration();

            this.LoadConfiguration();
            this.InitializeResultStorage();
            this.InitializeScheduler();
        }

        protected override void OnStop()
        {
            this.scheduler.Stop();
            this.nemesis.Stop();
        }

        private void InitializeResultStorage()
        {
            this.log.Info("Initializing Result Storage...");
            this.env.ResultsPath = Path.Combine(this.env.DataPath, "Results");

            this.resultStorage = new ResultStorage();
            ServiceProvider.SetService(this.resultStorage);
        }

        private void InitializeNemesis()
        {
#if DEBUG
            // This is only to prevent errors while the core launches.
            Thread.Sleep(TimeSpan.FromSeconds(3));
#endif
            this.log.Info("Initializing Nemesis...");
            this.env.NemesisConfigFile = Path.Combine(this.env.DataPath, "nemesis.yaml");
            this.env.NemesisKeyFile = Path.Combine(this.env.DataPath, "agent-keys.xml");
            this.nemesis = new NemesisController();
            ServiceProvider.SetService(this.nemesis);
        }

        private void UpdateConfiguration()
        {
            this.log.Info("Updating configuration...");

            // var config = this.nemesis.RetrieveString("getAgentConfig");
        }

        private void LoadConfiguration()
        {
            this.log.Debug("Loading configuration...");
            this.env.ConfigFile = Path.Combine(this.env.DataPath, "config.yaml");
            if (!File.Exists(this.env.ConfigFile))
            {
                this.log.Warn("No configuration file has been fetched. Creating an empty one.");

                var serializer = new SerializerBuilder()
                    .WithNamingConvention(new CamelCaseNamingConvention())
                    .Build();
                using (StreamWriter writer = new StreamWriter(this.env.ConfigFile))
                {
                    serializer.Serialize(writer, new AgentConfig());
                }
            }

            this.config = AgentConfigHandler.LoadConfigFromFile(this.env.ConfigFile);
            ServiceProvider.SetService(this.config);

            this.log.Debug("Configuration successfully loaded, found {0} checks.", this.config.Checks.Count);
        }

        private void InitializeScheduler()
        {
            this.log.Info("Initializing Scheduler...");
            this.scheduler = new AgentScheduler();
            this.scheduler.Start();
        }
    }
}
