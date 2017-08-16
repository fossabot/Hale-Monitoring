namespace Hale.Core.Handlers
{
    using Hale.Core.Handlers.Scheduler;
    using Hale.Lib;

    internal class QueuedInternalTask : QueuedTask
    {
        public override string Id => this.Task.ToString();

        public InternalTask Task { get; set; }

        public InternalTaskType TaskType => this.Task.Type;
    }
}
