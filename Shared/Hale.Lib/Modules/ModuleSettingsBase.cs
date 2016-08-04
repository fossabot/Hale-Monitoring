using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules
{
    [Serializable]
    public abstract class ModuleSettingsBase
    {
        protected Dictionary<string, string> _raw = new Dictionary<string, string>();

        public string Module { get; set; }

        public string Function { get; set; } = "default";

        public TimeSpan Interval { get; set; }

        public bool Enabled { get; set; } = true;

        public bool Startup { get; set; } = false;

        public Dictionary<string, Dictionary<string, string>> TargetSettings { get; set; }
            = new Dictionary<string, Dictionary<string, string>>();

        public ICollection<string> Targets { get { return TargetSettings.Keys; } }

        public bool Targetless
        {
            get
            {
                return TargetSettings == null ||
                    TargetSettings.Count == 0 ||
                    (TargetSettings.Count==1 && TargetSettings.ContainsKey("default"));
            }
        }

        public virtual void ParseRaw()
        {
            Module = _raw["module"];
            Interval = _raw.ContainsKey("interval") ? TimeSpan.Parse(_raw["interval"]) : TimeSpan.FromMinutes(10);
            Enabled = _raw.ContainsKey("enabled") ? bool.Parse(_raw["enabled"]) : true;
            Startup = _raw.ContainsKey("startup") ? bool.Parse(_raw["startup"]) : false;
        }

        public virtual string this[string key]
        {
            get { return _raw[key]; }
            set { _raw[key] = value; }
        }

        public virtual bool ContainsKey(string key)
        {
            return _raw.ContainsKey(key);
        }

    }

}
