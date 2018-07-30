namespace Hale.Agent.Scheduler
{
    using Hale.Lib;

    internal class QueuedModuleTask : QueuedTask
    {
        public override string Id =>
            this.Task.ToString();

        public ModuleTask Task { get; set; }
    }
}
