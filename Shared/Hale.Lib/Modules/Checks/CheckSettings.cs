namespace Hale.Lib.Modules.Checks
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class CheckSettings : ModuleSettingsBase
    {
        public CheckSettings()
        {
        }

        public CheckSettings(Dictionary<string, string> rawMap)
        {
            this.Raw = rawMap;
        }

        public CheckThresholds Thresholds { get; set; }

        public CheckActions Actions { get; set; }

        public string Check
        {
            get { return this.Function; } set { this.Function = value; }
        }

        public override void ParseRaw()
        {
            base.ParseRaw();
            this.Check = this.Raw.ContainsKey("check") ? this.Raw["check"] : "default";
        }
    }
}
