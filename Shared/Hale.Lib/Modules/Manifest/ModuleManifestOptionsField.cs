namespace Hale.Lib.Modules.Manifest
{
    public class ModuleManifestOptionsField
    {
        public ModuleManifestOptionType Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public object Default { get; set; }

        public bool Required { get; set; }

        public ModuleManifestOptionsGroup Group { get; set; }

        public double Max { get; set; }

        public double Min { get; set; }

        public bool Hidden { get; set; }
    }
}
