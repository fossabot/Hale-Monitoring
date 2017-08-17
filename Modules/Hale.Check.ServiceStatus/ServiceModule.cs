namespace Hale.Modules
{
    using System;
    using System.Collections.Generic;
    using System.ServiceProcess;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Actions;
    using Hale.Lib.Modules.Attributes;
    using Hale.Lib.Modules.Checks;
    using Hale.Lib.Modules.Info;

    [HaleModule("com.itshale.core.service", 0, 1, 1)]
    [HaleModuleName("Service Module")]
    public partial class ServiceModule : Module, ICheckProvider, IInfoProvider, IActionProvider
    {
        private static readonly ServiceControllerStatus[] CriticalStatuses =
        {
            ServiceControllerStatus.Stopped,
            ServiceControllerStatus.StopPending
        };

        public override string Name => "Service";

        public override string Author => "Hale Project";

        public override string Identifier => "com.itshale.core.service";

        public override string Platform => "Windows";

        public override decimal TargetApi => 1.2M;

        public override Version Version => new Version(0, 1, 1);

        Dictionary<string, ModuleFunction> IModuleProviderBase.Functions { get; set; }
            = new Dictionary<string, ModuleFunction>();

        public void InitializeCheckProvider(CheckSettings settings)
        {
            this.AddCheckFunction(this.ServiceRunningCheck);
            this.AddCheckFunction("running", this.ServiceRunningCheck);
        }

        public void InitializeInfoProvider(InfoSettings settings)
        {
            this.AddInfoFunction(this.ListServicesInfo);
            this.AddInfoFunction("list", this.ListServicesInfo);
        }

        public void InitializeActionProvider(ActionSettings settings)
        {
            this.AddActionFunction("start", this.StartServiceAction);
            this.AddActionFunction("stop", this.StopServiceAction);
            this.AddActionFunction("restart", this.RestartServiceAction);
        }
    }
}
