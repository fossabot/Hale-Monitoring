namespace Hale.Core
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using Hale.Core.Config;
    using Hale.Core.Handlers;
    using Hale.Lib.Utilities;
    using NLog;

#if NETCOREAPP2_1
    using Hale.Lib.Facades;
#else
    using System.ServiceProcess;
#endif

    /// <summary>
    /// The main entry point for starting the Hale-Core service.
    /// </summary>
    public partial class HaleCoreService : ServiceBase
    {
        private readonly Logger log;
        private AgentHandler agentHandler;
        private CoreConfig config;

        private EnvironmentConfig env;

        /// <summary>
        /// Initializes a new instance of the <see cref="HaleCoreService"/> class.
        /// </summary>
        public HaleCoreService()
        {
            this.log = LogManager.GetCurrentClassLogger();
            var v = Assembly.GetExecutingAssembly().GetName().Version;
            this.log.Info($"Hale Core v{v.Major}.{v.Minor}.{v.Revision} build {v.Build}");

            this.InitializeComponent();

            this.env = new EnvironmentConfig();
            this.env.DataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Hale\Core\";
            this.env.AgentDistPath = Path.Combine(this.env.DataPath, "dist");
            this.env.ModulePath = Path.Combine(this.env.DataPath, "modules");
            this.env.ConfigFile = Path.Combine(this.env.DataPath, "HaleCore.yaml");
            ServiceProvider.SetService(this.env);
        }

#if DEBUG
        /// <summary>
        /// Start in debugger mode, which enables us to start Hale-Core as a terminal app instead of a windows service, which is the default behaviour.
        /// </summary>
        public void DebugStart()
        {
            this.OnStart(new string[] { });
            while (Console.ReadLine() != "exit")
            {
            }
        }
#endif

        /// <summary>
        /// Method used for executing a threaded start of the core.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += this.ThreadedStart;
            worker.RunWorkerAsync();
        }

        protected override void OnStop()
        {
            // TODO: Handle stopping of service -SA 2017-08-16
        }

        private void ThreadedStart(object sender, DoWorkEventArgs args)
        {
            this.log.Info("Reading configuration...");

            if (File.Exists(this.env.ConfigFile))
            {
                this.log.Info($"Loaded configuration from '{this.env.ConfigFile}'.");
                this.config = CoreConfig.Load(this.env.ConfigFile);
            }
            else
            {
                this.log.Warn($"No configuration file present! Writing new file with defaults to '{this.env.ConfigFile}'.");
                this.config = CoreConfig.Default;
                this.config.Save(this.env.ConfigFile);
            }

            ServiceProvider.SetService(this.config);

#if DEBUG
            this.LaunchModuleHandler();
            this.LaunchCoreInstances();
#else
            LaunchCoreInstances();
#endif
        }

        private void LaunchModuleHandler()
        {
            ModuleHandler moduleHandler = new ModuleHandler();
            //moduleHandler.ScanForModules(this.env.ModulePath);
        }

        private void LaunchCoreInstances()
        {
            this.LaunchApiHandler();
            this.LaunchAgentHandler();

            // Distribution Handler is disabled for now -NM 2016-11-26
            // LaunchAgentDistributionHandler();
        }

        private void LaunchAgentHandler()
        {
            this.log.Info("Creating Agent Handler instance...");
            this.agentHandler = new AgentHandler();
            ServiceProvider.SetService(this.agentHandler);
        }

        private void LaunchAgentDistributionHandler()
        {
            this.log.Info("Creating Agent distribution handler...");
            AgentDistHandler distHandler = new AgentDistHandler();
            ServiceProvider.SetService(distHandler);

            this.log.Info("Creating Agent dist packages...");
            distHandler.CreatePackages();
        }

        private void LaunchApiHandler()
        {
            this.log.Info("Creating API Handler instance...");
            ApiHandler apiHandler = new ApiHandler();
        }
    }
}
