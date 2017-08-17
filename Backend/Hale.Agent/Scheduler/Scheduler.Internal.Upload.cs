namespace Hale.Agent.Scheduler
{
    using Hale.Agent.Communication;
    using Hale.Agent.Modules;
    using Hale.Lib.Utilities;

    internal partial class AgentScheduler
    {
        private void InternalTaskUploadResults()
        {
            var nemesis = ServiceProvider.GetService<NemesisController>();
            if (nemesis == null)
            {
                return;
            }

            var resultStorage = ServiceProvider.GetService<IResultStorage>();
            if (resultStorage == null)
            {
                return;
            }

            var records = resultStorage.Fetch(20);
            var uploaded = nemesis.UploadResults(records);

            if (uploaded != null)
            {
                resultStorage.Clear(uploaded);
            }
        }
    }
}
