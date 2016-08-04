using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Info
{
    [Serializable]
    public class InfoSettings: ModuleSettingsBase
    {
        public string Info { get; set; } = "default";

        public InfoSettings() { }
        public InfoSettings(Dictionary<string, string> rawMap)
        {
            _raw = rawMap;
        }

        public override void ParseRaw()
        {
            base.ParseRaw();
            Info = _raw.ContainsKey("info") ? _raw["info"] : "default";
        }
    }

}
