namespace Hale.Lib.Modules.Manifest
{
    using System.Collections.Generic;

    public class ModuleManifestOptions
    {
        public Dictionary<string, ModuleManifestOptionsField> Fields { get; set; }

        public Dictionary<string, ModuleManifestOptionsField> Builtin { get; set; }

        public List<ModuleManifestOptionsGroup> Groups { get; set; }
    }
}
