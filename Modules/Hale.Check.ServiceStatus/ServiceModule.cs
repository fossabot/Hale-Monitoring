using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using Hale.Lib.Modules;
using Hale.Lib.Modules.Checks;
using Hale.Lib.Modules.Actions;
using Hale.Lib.Modules.Info;
using System.Management;
using Hale.Lib.Modules.Attributes;

namespace Hale.Modules
{
    [HaleModule("com.itshale.core.service", 0, 1, 1)]
    [HaleModuleName("Service Module")]
    public partial class ServiceModule: Module, ICheckProvider, IInfoProvider, IActionProvider
    {
        public override string Name => "Service";
        public override string Author => "Hale Project";
        public override string Identifier => "com.itshale.core.service";
        public override string Platform => "Windows";
        public override decimal TargetApi => 1.2M;
        public override Version Version => new Version(0, 1, 1);

        Dictionary<string, ModuleFunction> IModuleProviderBase.Functions { get; set; }
            = new Dictionary<string, ModuleFunction>();

        static readonly ServiceControllerStatus[] _criticalStatuses = {
            ServiceControllerStatus.Stopped,
            ServiceControllerStatus.StopPending
        };


        public void InitializeCheckProvider(CheckSettings settings)
        {
            this.AddCheckFunction(ServiceRunningCheck);
            this.AddCheckFunction("running", ServiceRunningCheck);
        }

        public void InitializeInfoProvider(InfoSettings settings)
        {
            this.AddInfoFunction(ListServicesInfo);
            this.AddInfoFunction("list", ListServicesInfo);
        }

        public void InitializeActionProvider(ActionSettings settings)
        {
            this.AddActionFunction("start", StartServiceAction);
            this.AddActionFunction("stop", StopServiceAction);
            this.AddActionFunction("restart", RestartServiceAction);
        }
    }
}
