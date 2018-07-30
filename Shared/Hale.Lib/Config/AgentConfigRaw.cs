namespace Hale.Lib.Config
{
    using System.Collections.Generic;

    internal class AgentConfigRaw : AgentConfigBase
    {
        public List<Dictionary<string, object>> Checks { get; set; }
            = new List<Dictionary<string, object>>();

        public List<Dictionary<string, object>> Info { get; set; }
            = new List<Dictionary<string, object>>();

        public List<Dictionary<string, object>> Actions { get; set; }
            = new List<Dictionary<string, object>>();
    }
}
