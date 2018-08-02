namespace Hale.Lib.Modules.Results
{
    using System;
    using Hale.Lib.Modules.Exceptions;

    [Serializable]
    public class ModuleResult
    {
        private Exception executionException;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "We do not throw a new exception, only return it")]
        public Exception ExecutionException
        {
            get => this.executionException != null
            ? this.executionException
            : this.executionException = new UnknownUnsuccessfulException();
            set => this.executionException = value;
        }

        public bool RanSuccessfully { get; set; }

        public string Message { get; set; }

        public string Target { get; set; }
    }
}
