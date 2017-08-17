namespace Hale.Lib.Modules.Manifest
{
    using System;

    public class ModuleManifestInformation
    {
        public string Name { get; set; }

        public string Identifier { get; set; }

        public Version Version { get; set; }

        public string Website { get; set; }

        public ModuleManifestAuthor Author { get; set; }
    }
}
