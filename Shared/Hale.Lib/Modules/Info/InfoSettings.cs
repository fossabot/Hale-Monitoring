namespace Hale.Lib.Modules.Info
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class InfoSettings : ModuleSettingsBase
    {
        public InfoSettings()
        {
        }

        public InfoSettings(Dictionary<string, string> rawMap)
        {
            this.Raw = rawMap;
        }

        public string Info { get; set; } = "default";

        public override void ParseRaw()
        {
            base.ParseRaw();
            this.Info = this.Raw.ContainsKey("info") ? this.Raw["info"] : "default";
        }
    }
}
