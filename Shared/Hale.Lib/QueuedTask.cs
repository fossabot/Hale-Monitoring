namespace Hale.Lib
{
    using System;

    public class QueuedTask : IEquatable<QueuedTask>
    {
        public DateTime Added { get; set; }

        public DateTime Started { get; set; }

        public DateTime Completed { get; set; }

        public virtual string Id { get; set; }

        public bool Equals(QueuedTask other)
        {
            // Hack: Makes the comparision between QueuedTasks depend solely on the underlying CheckTask
            // so that TaskQueue.Contains() works correctly -NM
            return other.Id == this.Id;
        }
    }
}
