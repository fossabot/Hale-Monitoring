namespace Hale.Lib.Modules.Actions
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class ActionSettings : ModuleSettingsBase
    {
        public ActionSettings()
        {
        }

        public ActionSettings(Dictionary<string, string> rawMap)
        {
            this.Raw = rawMap;
        }

        public string Action { get; set; } = "default";

        public override void ParseRaw()
        {
            base.ParseRaw();
            this.Action = this.Raw.ContainsKey("action") ? this.Raw["action"] : "default";
        }
    }
}
