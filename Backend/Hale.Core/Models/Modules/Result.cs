using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Models.Modules
{
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
        public DateTime ExecutionTime { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Exception { get; set; }
    }
}
