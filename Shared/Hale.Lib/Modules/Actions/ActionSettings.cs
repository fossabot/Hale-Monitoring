using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Actions
{
    [Serializable]
    public class ActionSettings: ModuleSettingsBase
    {
        public string Action { get; set; } = "default";

        public ActionSettings() { }
        public ActionSettings(Dictionary<string, string> rawMap)
        {
            _raw = rawMap;
        }

        public override void ParseRaw()
        {
            base.ParseRaw();
            Action = _raw.ContainsKey("action") ? _raw["action"] : "default";
        }

    }

}
