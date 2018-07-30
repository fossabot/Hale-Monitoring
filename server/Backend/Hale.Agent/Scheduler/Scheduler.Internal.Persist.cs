namespace Hale.Agent.Scheduler
{
    using Hale.Agent.Modules;
    using Hale.Lib.Utilities;

    internal partial class AgentScheduler
    {
        private void InternalTaskPersistResults()
        {
            var resultStorage = ServiceProvider.GetService<IResultStorage>();
            if (resultStorage == null)
            {
                return;
            }

            resultStorage.Persist();
        }
    }
}
