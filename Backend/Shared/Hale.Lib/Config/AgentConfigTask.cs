namespace Hale.Lib.Config
{
    using System;
    using YamlDotNet.Serialization;

    public class AgentConfigTask
    {
        [YamlIgnore]
        public int Id { get; set; }

        public bool Enabled { get; set; } = true;

        public TimeSpan Interval { get; set; }

        public bool Startup { get; set; } = false;
    }
}
