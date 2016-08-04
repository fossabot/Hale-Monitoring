using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Checks
{
    [Serializable]
    public class CheckSettings: ModuleSettingsBase
    {

        public CheckThresholds Thresholds { get; set; }
        public CheckActions Actions { get; set; }

        public string Check { get { return Function; } set { Function = value; } }

        public CheckSettings() { }
        public CheckSettings(Dictionary<string, string> rawMap)
        {
            _raw = rawMap;
        }

        public override void ParseRaw()
        {
            base.ParseRaw();
            Check = _raw.ContainsKey("check") ? _raw["check"] : "default";
        }
    }

    [Serializable]
    public class CheckThresholds
    {
        public float Warning { get; set; }
        public float Critical { get; set; }
    }

    [Serializable]
    public class CheckActions
    {
        public CheckAction Warning { get; set; }
        public CheckAction Critical { get; set; }
    }

    [Serializable]
    public class CheckAction
    {
        public string Action { get; set; }
        public string Module { get; set; }
        public string Target { get; set; }
    }
}
