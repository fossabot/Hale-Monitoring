namespace Hale.Lib.Modules.Manifest
{
    using System.Collections.Generic;

    public class ModuleManifest
    {
        public ModuleManifestInformation Information { get; set; }

        public ModuleManifestModule Module { get; set; }

        public ModuleManifestOptions Options { get; set; }

        public Dictionary<string, ModuleManifestMetric> Metrics { get; set; }
    }
}
