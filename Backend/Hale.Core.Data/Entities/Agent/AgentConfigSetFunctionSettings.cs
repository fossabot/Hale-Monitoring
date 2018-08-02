#pragma warning disable 108, 114

namespace Hale.Core.Data.Entities.Agent
{
    public class AgentConfigSetFunctionSettings
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Target { get; set; }

        public int AgentConfigSetFunctionId { get; set; }
        public AgentConfigSetFunction AgentConfigSetFunction { get; set; }

    }
}
