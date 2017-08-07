using Hale.Lib.Modules;
using System;

namespace Hale.Agent.Modules
{
    internal interface IResultStorage
    {
        void StoreResult(IModuleResultRecord record);
        void Persist();
        ResultRecordChunk Fetch(int maxRecords);
        void Clear(Guid[] uploaded);
    }

    internal static class ResultStorageExtensions
    {
        public static void StoreResult(this IResultStorage rs, QueuedModuleTask task, ModuleFunctionResult result)
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
