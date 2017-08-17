namespace Hale.Agent.Scheduler
{
    using Hale.Agent.Communication;
    using Hale.Lib.Utilities;

    internal partial class AgentScheduler
    {
        private void InternalTaskSendHeartbeat()
        {
            var nemesis = ServiceProvider.GetService<NemesisController>();
            if (nemesis == null)
            {
                return;
            }

            this.Log.Debug("Sending hearbeat to Core...");
            var response = nemesis.SendCommand("heartbeat");
            if (response == "OK")
            {
                return;
            }
            else if (response == "UnknownGUID")
            {
                this.Log.Info($"Core responded with \"{response}\". Sending identification...");
                this.EnqueueTask(new InternalTask(InternalTaskType.IdentifyAgent));
            }
        }
    }
}
