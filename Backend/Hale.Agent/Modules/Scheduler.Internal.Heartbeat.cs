using Hale.Agent.Communication;
using Hale.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Agent.Modules
{
    partial class AgentScheduler
    {
        private void internalTaskSendHeartbeat()
        {
            var nemesis = ServiceProvider.GetService<NemesisController>();
            if (nemesis == null) return;
            _log.Debug("Sending hearbeat to Core...");
            var response = nemesis.SendCommand("heartbeat");
            if (response == "OK") return;
            else if(response == "UnknownGUID")
            {
                _log.Info($"Core responded with \"{response}\". Sending identification...");
                this.EnqueueTask(new InternalTask(InternalTaskType.IdentifyAgent));
            }
        }
    }
}
