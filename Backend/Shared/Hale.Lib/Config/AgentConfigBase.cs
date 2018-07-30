namespace Hale.Lib.Config
{
    using System.Collections.Generic;

    public abstract class AgentConfigBase : AgentConfigBare
    {
        public Dictionary<string, AgentConfigModule> Modules { get; set; }

        public Dictionary<string, AgentConfigTask> Tasks { get; set; } = new Dictionary<string, AgentConfigTask>();
    }
}
