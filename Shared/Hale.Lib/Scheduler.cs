using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timers = System.Timers;

namespace Hale.Lib
{
    public abstract class Scheduler
    {
        protected ILogger _log = LogManager.GetLogger("Scheduler");

        public Dictionary<TimeSpan, List<TaskBase>> ScheduleTasks =
            new Dictionary<TimeSpan, List<TaskBase>>();

        public ConcurrentQueue<QueuedTask> TaskQueue = new ConcurrentQueue<QueuedTask>();

        protected int _taskWorkerCount = 5; // NM: should move to config @hardcoded

        protected List<BackgroundWorker> TaskWorkerPool = new List<BackgroundWorker>();

        public Dictionary<TimeSpan, Timers.Timer> TaskTimers = new Dictionary<TimeSpan, Timers.Timer>();

        bool _stopping = false;

        protected abstract void RunTask(QueuedTask queuedTask);

        public Scheduler()
        {

        }

        public void ScheduleTask(TaskBase task, TimeSpan interval)
        {
            if (!ScheduleTasks.ContainsKey(interval))
            {
                ScheduleTasks.Add(interval, new List<TaskBase>());
            }
            ScheduleTasks[interval].Add(task);
        }

        protected void PrintTasks() { 
#if DEBUG
            _log.Debug("Scheduled tasks:");
            foreach (var interval in ScheduleTasks)
            {

                _log.Debug("  {0}", interval.Key.ToString());
                foreach (var task in interval.Value)
                {
                    _log.Debug($"    {task}");
                }
            }
#endif
        }

        public void Start()
        {
            _updateQueue();
            _stopping = false;

            // NM: Perhaps some thread-safety should be added @ts @fixme
            var _thread = new Thread(() =>
            {
                while (TaskWorkerPool.Count < _taskWorkerCount)
                {
                    var bw = new BackgroundWorker();
                    bw.DoWork += (s, e) => RunTask((QueuedTask)(e.Argument));
                    TaskWorkerPool.Add(bw);
                }

                while (!_stopping) // @ts?
                {
                    if (TaskQueue.Count > 0)
                    {
                        var worker = TaskWorkerPool.FirstOrDefault(bw => !bw.IsBusy);
                        if (worker != null)
                        {
                            QueuedTask task;
                            if (TaskQueue.TryDequeue(out task))
                            {
                                worker.RunWorkerAsync(task);
                            }
                            else
                            {
                                _log.Debug("Queue is locked by another thread.");
                                Thread.Sleep(200); // NM: Shortest wait when Queue is thread-locked @hardcoded 
                                continue;
                            }
                        }
                        else
                        {
                            _log.Debug("Pool is busy.");
                            Thread.Sleep(1000); // NM: Waiting shorter time if pool is busy @hardcoded
                            continue;
                        }
                    }
                    else
                    {
                        //_log.Debug("Nothing queued.");
                        Thread.Sleep(5000); // NM: Waiting longer time if nothing is queued @hardcoded
                        continue;
                    }
                }
            });
            _thread.Start();

        }

        public void Stop(bool force = false)
        {
            _stopping = true;
            if (force)
            {
                foreach (var bw in TaskWorkerPool)
                {
                    bw.CancelAsync();
                }
            }
        }
        
        protected void _updateQueue()
        {
            if (TaskTimers.Count > 0)
            {
                foreach (var timer in TaskTimers)
                {
                    timer.Value.Stop();
                }
                TaskTimers.Clear();

            }

            foreach (var kvpCheckTask in ScheduleTasks)
            {
                try
                {
                    var timer = new Timers.Timer(kvpCheckTask.Key.TotalMilliseconds); // Maximum interval is 24 days (Int.Max milliseconds)

                    timer.Elapsed += delegate { EnqueueTasks(kvpCheckTask.Value); };

                    timer.Start();

                    TaskTimers.Add(kvpCheckTask.Key, timer);
                }
                catch (Exception x)
                {
                    _log.Error($"Could not add Task interval {kvpCheckTask.Key}: {x.Message}");
                }
            }

        }

        protected void EnqueueTasks(List<TaskBase> tasks)
        {
            _log.Debug($"Enqueueing {tasks.Count} task(s).");
            foreach (var task in tasks)
            {
                EnqueueTask(task);
            }
        }

        protected void EnqueueTask(TaskBase task)
        {
            var queuedTask = task.ToQueued();

            if (TaskQueue.Contains(queuedTask))
            {
                // Todo: Handle dead-locked tasks @todo -NM
                _log.Warn($"Skipping task {task}, previous task not completed.");
            }
            else
            {
                queuedTask.Added = DateTime.Now;
                TaskQueue.Enqueue(queuedTask);
                _log.Debug($"Enqueued task {task}.");
            }
        }
    }

    public abstract class TaskBase
    {
        public abstract QueuedTask ToQueued();
    }

    public class QueuedTask : IEquatable<QueuedTask>
    {
        public bool Equals(QueuedTask other)
        {
            // Hack: Makes the comparision between QueuedTasks depend solely on the underlying CheckTask
            // so that TaskQueue.Contains() works correctly -NM
            return other.Id == this.Id;
        }
        public virtual string Id { get; set; }
        public DateTime Added;
        public DateTime Started;
        public DateTime Completed;
    }

}
