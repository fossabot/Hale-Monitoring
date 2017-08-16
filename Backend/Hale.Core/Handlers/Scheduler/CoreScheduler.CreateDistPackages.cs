namespace Hale.Core.Handlers.Scheduler
{
    using Hale.Lib.Utilities;

    internal partial class CoreScheduler
    {
        private void CreateDistPackages()
        {
            var distHandler = ServiceProvider.GetService<AgentDistHandler>();
            if (distHandler == null)
            {
                return;
            }

            distHandler.CreatePackages();
        }
    }
}
