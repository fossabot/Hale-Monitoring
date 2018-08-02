#pragma warning disable 108, 114

namespace Hale.Core.Data.Entities.Agent
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public class AgentConfigSet
    {
        public static AgentConfigSet Empty => new AgentConfigSet()
        {
            Functions = new List<AgentConfigSetFunction>(),
            Tasks = new List<AgentConfigSetTask>(),
        };

        /// <summary>
        /// Databse row ID
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier for this configuration
        /// </summary>
        [StringLength(32)]
        public string Identifier { get; set; }

        /// <summary>
        /// A human-readable name for the configuration which will be displayed in UIs
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// TODO: Creation time
        /// </summary>
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public DateTimeOffset? Modified { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public List<AgentConfigSetFunction> Functions { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public List<AgentConfigSetTask> Tasks { get; set; }
    }
}
