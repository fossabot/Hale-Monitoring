namespace Hale.Core.Handlers.Scheduler
{
    using System;
    using Hale.Lib;

    internal partial class CoreScheduler : Scheduler
    {
        public void RunTask(InternalTaskType taskType)
        {
            var task = new InternalTask(taskType);
            this.EnqueueTask(task);
        }

        public void ScheduleTask(InternalTaskType taskType, TimeSpan interval)
        {
            var task = new InternalTask(taskType);
            this.ScheduleTask(task, interval);
        }

        protected override void RunTask(QueuedTask task)
        {
            var qiTask = (QueuedInternalTask)task;
            switch (qiTask.TaskType)
            {
                case InternalTaskType.CreateDistPackages:
                    this.CreateDistPackages();
                    break;
            }

            qiTask.Completed = DateTime.UtcNow;
        }
    }
}
