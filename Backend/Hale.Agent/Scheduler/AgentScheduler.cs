namespace Hale.Agent.Scheduler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Hale.Agent.Config;
    using Hale.Agent.Modules;
    using Hale.Lib;
    using Hale.Lib.Config;
    using Hale.Lib.ModuleLoader;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Results;
    using Hale.Lib.Utilities;

    internal partial class AgentScheduler : Scheduler
    {
        public AgentScheduler()
        {
            var env = ServiceProvider.GetService<EnvironmentConfig>();
            env.ModulePath = Path.Combine(env.DataPath, "Modules");

            this.ProcessConfig();

            this.PrintTasks();
        }

        protected override void RunTask(QueuedTask queuedTask)
        {
            this.Log.Info($"Running task {queuedTask.Id}");
            if (queuedTask.GetType() == typeof(QueuedModuleTask))
            {
                this.RunModuleTask(queuedTask as QueuedModuleTask);
            }
            else if (queuedTask.GetType() == typeof(QueuedInternalTask))
            {
                var task = (QueuedInternalTask)queuedTask;
                switch (task.TaskType)
                {
                    case InternalTaskType.PersistResults:
                        this.InternalTaskPersistResults();
                        break;
                    case InternalTaskType.UploadResults:
                        this.InternalTaskUploadResults();
                        break;
                    case InternalTaskType.SendHeartbeat:
                        this.InternalTaskSendHeartbeat();
                        break;
                    case InternalTaskType.IdentifyAgent:
                        this.InternalTaskIdentifyAgent();
                        break;
                    default:
                        break;
                }
            }
        }

        private void ProcessInternalTasks(AgentConfig config)
        {
            foreach (InternalTaskType itt in Enum.GetValues(typeof(InternalTaskType)))
            {
                var key = itt.ToString();
                key = key.Substring(0, 1).ToLower() + key.Substring(1); // Convert to dict key
                if (!config.Tasks.ContainsKey(key) || !config.Tasks[key].Enabled)
                {
                    continue;
                }

                this.ScheduleTask(new InternalTask(itt), config.Tasks[key].Interval);
                if (config.Tasks[key].Startup)
                {
                    this.EnqueueTask(new InternalTask(itt));
                }
            }
        }

        private void ProcessConfig()
        {
            var config = ServiceProvider.GetServiceCritical<AgentConfig>();

            // VerifyChecks(config.Checks);
            config.Checks.ForEach(check => this.ProcessConfigFunction(check, ModuleFunctionType.Check));
            config.Info.ForEach(info => this.ProcessConfigFunction(info, ModuleFunctionType.Info));
            config.Actions.ForEach(action => this.ProcessConfigFunction(action, ModuleFunctionType.Action));

            this.ProcessInternalTasks(config);
        }

        private void ProcessConfigFunction(ModuleSettingsBase function, ModuleFunctionType type)
        {
            if (!function.Startup && !function.Enabled)
            {
                return;
            }

            var task = new ModuleTask()
            {
                FunctionType = type,
                Function = function.Function,
                Module = function.Module.ToString(),
                Settings = function
            };

            if (task.Settings.Startup)
            {
                this.EnqueueTask(task);
            }

            if (task.Settings.Enabled)
            {
                this.ScheduleTask(task, function.Interval);
            }
        }

        private void RunModuleTask(QueuedModuleTask queuedTask)
        {
            var config = ServiceProvider.GetService<AgentConfig>();
            if (config == null)
            {
                return;
            }

            var resultStorage = ServiceProvider.GetService<IResultStorage>();
            if (resultStorage == null)
            {
                return;
            }

            var task = queuedTask.Task;
            try
            {
                var checkPath = this.GetModulePath(task.Module);
                var dll = Path.Combine(checkPath, config.Modules[task.Module].Dll);

                if (!File.Exists(dll))
                {
                    throw new FileNotFoundException($"Check DLL \"{dll}\" not found!");
                }

                ModuleResultSet functionResult;
                switch (task.FunctionType)
                {
                    case ModuleFunctionType.Check:
                        functionResult = ModuleLoader.ExecuteFunction<CheckResultSet>(dll, checkPath, task.Function, task.Settings);
                        this.LogCheckResults(task, functionResult);
                        break;
                    case ModuleFunctionType.Info:
                        functionResult = ModuleLoader.ExecuteFunction<InfoResultSet>(dll, checkPath, task.Function, task.Settings);
                        this.LogInfoResult(task, functionResult);
                        break;
                    case ModuleFunctionType.Action:
                        functionResult = ModuleLoader.ExecuteFunction<ActionResultSet>(dll, checkPath, task.Function, task.Settings);
                        this.LogActionResult(task, functionResult);
                        break;
                    default:
                        throw new ArgumentException($"Incorrect function type \"{task.FunctionType}\".");
                }

                queuedTask.Completed = DateTime.UtcNow;
                resultStorage.StoreResult(queuedTask, functionResult);

                this.Log.Info($"The task {task} completed in {(queuedTask.Completed - queuedTask.Added).TotalSeconds.ToString("F2")} second(s)");
            }
            catch (Exception x)
            {
                this.Log.Error($"Error running Task {task}({string.Join(",", task.Targets)}): {x.Message}");
            }
        }

        private void LogActionResult(ModuleTask task, ModuleResultSet functionResult)
        {
            var actionResult = functionResult as ActionResultSet;
            foreach (var kvpResult in actionResult.ActionResults)
            {
                var result = kvpResult.Value;
                var target = kvpResult.Key;
                if (result.RanSuccessfully)
                {
                    this.Log.Info($"Task {task}({result.Target}) executed successfully!");
                }
                else
                {
                    this.Log.Warn($"Task {task}({result.Target}) executed with error: {result.ExecutionException.Message}");
                }

                this.Log.Info($"  -> {result.Message}");
            }
        }

        private void LogInfoResult(ModuleTask task, ModuleResultSet functionResult)
        {
            var infoResult = functionResult as InfoResultSet;
            foreach (var kvpResult in infoResult.InfoResults)
            {
                var result = kvpResult.Value;
                var target = kvpResult.Key;
                if (result.RanSuccessfully || result.ExecutionException == null)
                {
                    this.Log.Info($"Task {task}({result.Target}) executed successfully!");
                    foreach (var item in result.Items)
                    {
                        this.Log.Debug($" - {item.Key} = {item.Value}");
                    }
                }
                else
                {
                    this.Log.Warn($"Task {task}({result.Target}) executed with error: {result.ExecutionException.Message}");
                }

                this.Log.Info($"  -> {result.Message}");
            }
        }

        private void LogCheckResults(ModuleTask task, ModuleResultSet functionResult)
        {
            var checkResult = functionResult as CheckResultSet;
            foreach (var kvpResult in checkResult.CheckResults)
            {
                var result = kvpResult.Value;
                var target = kvpResult.Key;
                if (result.RanSuccessfully)
                {
                    this.Log.Info($"Task {task}({result.Target}) returned the raw values {result.RawValues}");
                }
                else
                {
                    this.Log.Warn($"Task {task}({result.Target}) executed with error: {result.ExecutionException.Message}");
                }

                this.Log.Info($"  -> {result.Message}");
            }
        }

        private string GetModulePath(string check)
        {
            var env = ServiceProvider.GetServiceCritical<EnvironmentConfig>();

            var matches = Regex.Match(check, @"(.*)\.(.*_.*)$");

            return Path.Combine(
                env.ModulePath,
                matches.Groups[1].Value,
                matches.Groups[2].Value);
        }

        private void VerifyChecks(Dictionary<string, ModuleSettingsBase> checks)
        {
            foreach (var check in checks)
            {
                var checkName = check.Key;
                var checkSettings = check.Value;

                // TODO: Check module signatures and checksum -NM 2017-08-16
            }
        }

        private new void EnqueueTasks(List<TaskBase> tasks)
        {
            this.Log.Debug($"Enqueueing {tasks.Count} task(s).");
            foreach (var task in tasks)
            {
                QueuedTask queuedTask = null;
                if (task.GetType() == typeof(ModuleTask))
                {
                    queuedTask = new QueuedModuleTask() { Task = (ModuleTask)task };
                }
                else if (task.GetType() == typeof(InternalTask))
                {
                    queuedTask = new QueuedInternalTask() { Task = (InternalTask)task };
                }
                else
                {
                    throw new Exception($"Internal scheduler error: Unknown Task Type {task.GetType().ToString()}.");
                }

                if (this.TaskQueue.Contains(queuedTask))
                {
                    // Todo: Handle dead-locked tasks @todo -NM
                    this.Log.Warn($"Skipping task {task}, previous task not completed.");
                }
                else
                {
                    queuedTask.Added = DateTime.UtcNow;
                    this.TaskQueue.Enqueue(queuedTask);
                }
            }
        }
    }
}
