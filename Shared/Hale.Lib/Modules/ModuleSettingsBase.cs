namespace Hale.Lib.Modules
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public abstract class ModuleSettingsBase
    {
        [YamlDotNet.Serialization.YamlMember(serializeAs: typeof(string))]
        public VersionedIdentifier Module { get; set; }

        public string Function { get; set; } = "default";

        public TimeSpan Interval { get; set; }

        public bool Enabled { get; set; } = true;

        public bool Startup { get; set; } = false;

        public ICollection<string> Targets
            => this.TargetSettings.Keys;

        public Dictionary<string, Dictionary<string, string>> TargetSettings { get; set; }
            = new Dictionary<string, Dictionary<string, string>>();

        public bool Targetless
            => this.TargetSettings?.Count == 0;

        [CLSCompliant(false)]
        protected Dictionary<string, string> Raw { get; set; } = new Dictionary<string, string>();

        public virtual string this[string key]
        {
            get { return this.Raw[key]; }
            set { this.Raw[key] = value; }
        }

        public virtual void ParseRaw()
        {
            this.Module = VersionedIdentifier.Parse(this.Raw["module"]);
            this.Interval = this.Raw.ContainsKey("interval") ? TimeSpan.Parse(this.Raw["interval"]) : TimeSpan.FromMinutes(10);
            this.Enabled = this.Raw.ContainsKey("enabled") ? bool.Parse(this.Raw["enabled"]) : true;
            this.Startup = this.Raw.ContainsKey("startup") ? bool.Parse(this.Raw["startup"]) : false;
        }

        public virtual bool ContainsKey(string key)
        {
            return this.Raw.ContainsKey(key);
        }
    }
}
