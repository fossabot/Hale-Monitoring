namespace Hale.Core.Data.Entities.Modules
{
    using System;

    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public class Result
    {
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int HostId { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int FunctionId { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public DateTimeOffset ExecutionTime { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Exception { get; set; }

        public bool AboveWarning { get; set; }

        public bool AboveCritical { get; set; }
    }
}
