namespace Hale.Lib.Modules.Alerts
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class AlertSettings : ModuleSettingsBase
    {
        public AlertSettings()
        {
        }

        public AlertSettings(Dictionary<string, string> rawMap)
        {
            this.Raw = rawMap;
        }

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
                return $"<{this.SourceModule.ToString()}[{this.SourceFunctionType.ToString().Substring(0, 1)}]{this.SourceFunction}({this.SourceTarget})>";
            }
        }

        public override void ParseRaw()
        {
            base.ParseRaw();
            this.Alert = this.Raw.ContainsKey("alert") ? this.Raw["alert"] : "default";
        }
    }
}
