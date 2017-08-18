namespace Hale.Lib
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using NLog;
    using Timers = System.Threading;

    public abstract class Scheduler
    {
        private bool stopping = false;

        public Scheduler()
        {
        }

        public Dictionary<TimeSpan, List<TaskBase>> ScheduleTasks { get; } = new Dictionary<TimeSpan, List<TaskBase>>();

        public ConcurrentQueue<QueuedTask> TaskQueue { get; } = new ConcurrentQueue<QueuedTask>();

        public Dictionary<TimeSpan, Timers.Timer> TaskTimers { get; } = new Dictionary<TimeSpan, Timers.Timer>();

        [CLSCompliant(false)]
        protected ILogger Log { get; } = LogManager.GetLogger("Scheduler");

        [CLSCompliant(false)]
        protected int TaskWorkerCount => 5; // NM: should move to config @hardcoded

        protected List<BackgroundWorker> TaskWorkerPool { get; } = new List<BackgroundWorker>();

        public void Start()
        {
            this.UpdateQueue();
            this.stopping = false;

            // NM: Perhaps some thread-safety should be added @ts @fixme
            var thread = new Thread(() =>
            {
                while (this.TaskWorkerPool.Count < this.TaskWorkerCount)
                {
                    var bw = new BackgroundWorker();
                    bw.DoWork += (s, e) => this.RunTask((QueuedTask)e.Argument);
                    this.TaskWorkerPool.Add(bw);
                }

                // @ts?
                while (!this.stopping)
                {
                    if (this.TaskQueue.Count > 0)
                    {
                        var worker = this.TaskWorkerPool.FirstOrDefault(bw => !bw.IsBusy);
                        if (worker != null)
                        {
                            QueuedTask task;
                            if (this.TaskQueue.TryDequeue(out task))
                            {
                                worker.RunWorkerAsync(task);
                            }
                            else
                            {
                                this.Log.Debug("Queue is locked by another thread.");
                                Thread.Sleep(200); // NM: Shortest wait when Queue is thread-locked @hardcoded
                                continue;
                            }
                        }
                        else
                        {
                            this.Log.Debug("Pool is busy.");
                            Thread.Sleep(1000); // NM: Waiting shorter time if pool is busy @hardcoded
                            continue;
                        }
                    }
                    else
                    {
                        // _log.Debug("Nothing queued.");
                        Thread.Sleep(5000); // NM: Waiting longer time if nothing is queued @hardcoded
                        continue;
                    }
                }
            });
            thread.Start();
        }

        public void Stop(bool force = false)
        {
            this.stopping = true;
            if (force)
            {
                foreach (var bw in this.TaskWorkerPool)
                {
                    bw.CancelAsync();
                }
            }
        }

        public void ScheduleTask(TaskBase task, TimeSpan interval)
        {
            if (!this.ScheduleTasks.ContainsKey(interval))
            {
                this.ScheduleTasks.Add(interval, new List<TaskBase>());
            }

            this.ScheduleTasks[interval].Add(task);
        }

        protected abstract void RunTask(QueuedTask queuedTask);

        protected void PrintTasks()
        {
#if DEBUG
            this.Log.Debug("Scheduled tasks:");
            foreach (var interval in this.ScheduleTasks)
            {
                this.Log.Debug("  {0}", interval.Key.ToString());
                foreach (var task in interval.Value)
                {
                    this.Log.Debug($"    {task}");
                }
            }
#endif
        }

        protected void UpdateQueue()
        {
            if (this.TaskTimers.Count > 0)
            {
                foreach (var timer in this.TaskTimers)
                {
                    timer.Value.Dispose();
                }

                this.TaskTimers.Clear();
            }

            foreach (var kvpCheckTask in this.ScheduleTasks)
            {
                try
                {
                    TimerCallback tcb = new TimerCallback(this.OnElapsedTime);
                    var timer = new Timer(tcb, kvpCheckTask.Value, kvpCheckTask.Key, kvpCheckTask.Key);

                    this.TaskTimers.Add(kvpCheckTask.Key, timer);
                }
                catch (Exception x)
                {
                    this.Log.Error($"Could not add Task interval {kvpCheckTask.Key}: {x.Message}");
                }
            }
        }

        protected void EnqueueTasks(List<TaskBase> tasks)
        {
            this.Log.Debug($"Enqueueing {tasks.Count} task(s).");
            foreach (var task in tasks)
            {
                this.EnqueueTask(task);
            }
        }

        protected void EnqueueTask(TaskBase task)
        {
            var queuedTask = task.ToQueued();

            if (this.TaskQueue.Contains(queuedTask))
            {
                // Todo: Handle dead-locked tasks @todo -NM
                this.Log.Warn($"Skipping task {task}, previous task not completed.");
            }
            else
            {
                queuedTask.Added = DateTime.UtcNow;
                this.TaskQueue.Enqueue(queuedTask);
                this.Log.Debug($"Enqueued task {task}.");
            }
        }

        private void OnElapsedTime(object stateInfo)
        {
            var tasks = stateInfo as List<TaskBase>;
            this.EnqueueTasks(tasks);
        }
    }
}
