#pragma warning disable 108, 114

namespace Hale.Core.Data.Entities.Agent
{
    using System;

    /// <summary>
    /// TODO: Add text here
    /// </summary>
    [Serializable]
    public class AgentConfigSetCheckAction
    {
        public int Id { get; set; }

        public string Action { get; set; }

        public string Module { get; set; }

        public string Target { get; set; }
    }
}
