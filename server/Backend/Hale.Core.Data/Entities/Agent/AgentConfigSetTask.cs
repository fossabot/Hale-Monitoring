#pragma warning disable 108, 114

namespace Hale.Core.Data.Entities.Agent
{
    using Hale.Lib.Config;

    public class AgentConfigSetTask : AgentConfigTask
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
