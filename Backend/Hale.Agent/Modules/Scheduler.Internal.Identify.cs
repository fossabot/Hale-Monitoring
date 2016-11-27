using Hale.Agent.Communication;
using Hale.Lib;
using Hale.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Agent.Modules
{
    partial class AgentScheduler
    {
        private void internalTaskIdentifyAgent()
        {
            var nemesis = ServiceProvider.GetService<NemesisController>();
            if (nemesis == null) return;

            var agentId = new AgentIdentification();
            agentId.Hostname = Environment.MachineName;
            agentId.OperatingSystem = ComputerInfo.GetOperatingSystemInfo();
            agentId.HardwareSummary = ComputerInfo.GetHardwareInfo();
            agentId.NetworkInterfaces = ComputerInfo.GetNetworkInterfaceInfo();

            nemesis.RetrieveString("identifyAgent", new object[] { agentId });
        }
    }
}
