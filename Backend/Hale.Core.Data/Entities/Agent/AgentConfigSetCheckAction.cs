using System;
#pragma warning disable 108, 114

namespace Hale.Core.Data.Entities.Agent
{
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    [Serializable]
    public class AgentConfigSetCheckAction
    {
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Action { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Module { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Target { get; set; }
    }
}
