namespace Hale.Agent.Scheduler
{
    using System;
    using Hale.Agent.Communication;
    using Hale.Lib;
    using Hale.Lib.Utilities;

    internal partial class AgentScheduler
    {
        private void InternalTaskIdentifyAgent()
        {
            var nemesis = ServiceProvider.GetService<NemesisController>();
            if (nemesis == null)
            {
                return;
            }

            var agentId = new AgentIdentification()
            {
                Hostname = Environment.MachineName,
                OperatingSystem = ComputerInfo.GetOperatingSystemInfo(),
                HardwareSummary = ComputerInfo.GetHardwareInfo(),
                NetworkInterfaces = ComputerInfo.GetNetworkInterfaceInfo()
            };

            nemesis.RetrieveString("identifyAgent", new object[] { agentId });
        }
    }
}
