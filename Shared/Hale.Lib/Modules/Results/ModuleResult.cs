namespace Hale.Lib.Modules.Results
{
    using System;
    using Hale.Lib.Modules.Exceptions;

    [Serializable]
    public class ModuleResult
    {
        private Exception executionException;

        #pragma warning disable CA1065 // Do not throw exception
        public Exception ExecutionException
        {
            get => this.executionException != null
            ? this.executionException
            : this.executionException = new UnknownUnsuccessfulException();
            set => this.executionException = value;
        }
        #pragma warning restore CA1065 // Do not throw exception

        public bool RanSuccessfully { get; set; }

        public string Message { get; set; }

        public string Target { get; set; }
    }
}
