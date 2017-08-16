namespace Hale.Core.Handlers.Scheduler
{
    using Hale.Lib;

    internal class InternalTask : TaskBase
    {
        public InternalTask()
        {
        }

        public InternalTask(InternalTaskType type)
        {
            this.Type = type;
        }

        public InternalTaskType Type { get; set; }

        public override string ToString()
        {
            return $"<com.itshale.agent[internal]{this.Type.ToString().ToLower()}()>";
        }

        public override QueuedTask ToQueued()
        {
            return new QueuedInternalTask() { Task = this };
        }
    }
}
