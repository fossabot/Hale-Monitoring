namespace Hale.Agent.Modules
{
    using System;
    using Hale.Agent.Scheduler;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Results;

    internal interface IResultStorage
    {
        void StoreResult(IModuleResultRecord record);

        void Persist();

        ResultRecordChunk Fetch(int maxRecords);

        void Clear(Guid[] uploaded);
    }

    internal static class ResultStorageExtensions
    {
        public static void StoreResult(this IResultStorage rs, QueuedModuleTask task, ModuleResultSet result)
        {
            rs.StoreResult(new ModuleResultRecord()
            {
                Module = result.Module,
                Runtime = result.Runtime,
                FunctionType = task.Task.FunctionType,
                Function = task.Task.Function,
                CompletionTime = task.Completed,
                Results = result.Results
            });
        }
    }
}
