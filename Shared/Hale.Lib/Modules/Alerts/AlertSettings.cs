using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Alerts
{
    [Serializable]
    public class AlertSettings : ModuleSettingsBase
    {
        public string Alert { get; set; } = "default";

        public string Message { get; set; }
        public VersionedIdentifier SourceModule { get; set; }
        public string SourceFunction { get; set; } = "default";
        public ModuleFunctionType SourceFunctionType { get; set; }
        public string SourceTarget { get; set; } = "default";

        public string SourceString
        {
            get
            {
                return $"<{SourceModule.ToString()}[{SourceFunctionType.ToString().Substring(0,1)}]{SourceFunction}({SourceTarget})>";
            }
        }

        public AlertSettings() { }
        public AlertSettings(Dictionary<string, string> rawMap)
        {
            _raw = rawMap;
        }

        public override void ParseRaw()
        {
            base.ParseRaw();
            Alert = _raw.ContainsKey("alert") ? _raw["alert"] : "default";
        }
    }
}
