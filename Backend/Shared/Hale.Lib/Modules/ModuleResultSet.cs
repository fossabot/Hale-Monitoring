namespace Hale.Lib.Modules
{
    using System;
    using System.Collections.Generic;
    using Hale.Lib.Modules.Exceptions;
    using Hale.Lib.Modules.Results;

    [Serializable]
    public class ModuleResultSet
    {
        private Exception functionException;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "We do not actually throw, just return an exception object.")]
        public Exception FunctionException
        {
            get
            {
                return (this.functionException != null)
                    ? this.functionException : new UnknownUnsuccessfulException();
            }

            set
            {
                this.functionException = value;
            }
        }

        public string Message { get; set; }

        public bool RanSuccessfully { get; set; }

        public virtual Dictionary<string, object> Results { get; set; } = new Dictionary<string, object>();

        public VersionedIdentifier Module { get; set; }

        public VersionedIdentifier Runtime { get; set; }

        private ICollection<string> Targets
        {
            get { return this.Results.Keys; }
        }

        public static TResult FromSingle<TResult>(ModuleResult moduleResult, string name)
            where TResult : ModuleResultSet, new()
        {
            return new TResult()
            {
                Results = new Dictionary<string, object>()
                 {
                     { name, moduleResult }
                 },
                RanSuccessfully = moduleResult.RanSuccessfully,
                Message = moduleResult.Message,
                FunctionException = moduleResult.ExecutionException
            };
        }
    }
}
