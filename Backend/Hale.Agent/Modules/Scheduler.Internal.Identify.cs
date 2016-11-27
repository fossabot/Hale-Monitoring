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
            agentId.OperatingSystem = Environment.OSVersion.VersionString;

            var inis = new List<IdNetworkInterface>();
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach(var nic in nics.Where( nic => 
                nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
            {
                var ini = new IdNetworkInterface();

                var ips = new List<string>();
                foreach (var ipi in nic.GetIPProperties().UnicastAddresses)
                {
                    ips.Add(ipi.Address.ToString());
                }
                ini.Addresses = ips.ToArray();

                ini.Name = nic.Description;
                ini.PhysicalAddress = string.Join("-",
                    nic.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2")));

                inis.Add(ini);
            }

            agentId.NetworkInterfaces = inis.ToArray();

            nemesis.RetrieveString("identifyAgent", new object[] { agentId });
        }
    }
}
