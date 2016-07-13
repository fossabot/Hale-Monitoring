using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules
{
    public class ModuleManifest
    {
        public ModuleManifestInformation Information { get; set; }
        public ModuleManifestModule Module { get; set; }
        public ModuleManifestOptions Options { get; set; }
        public Dictionary<string, ModuleManifestMetric> Metrics { get; set; }
    }

    public class ModuleManifestMetric
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double ExpectedMax { get; set; }
        public double ExpectedMin { get; set; }
        public string Unit { get; set; }
        public double ScaleMagnitude { get; set; }
        public string Resolution { get; set; }
    }

    public class ModuleManifestOptions
    {
        public Dictionary<string,ModuleManifestOptionsField> Fields { get; set; }
        public Dictionary<string, ModuleManifestOptionsField> Builtin { get; set; }
        public List<ModuleManifestOptionsGroup> Groups { get; set; }
    }

    public class ModuleManifestOptionsGroup
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Expandend { get; set; }
        public bool Hidden { get; set; }
    }

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

    public class ModuleManifestModule
    {
        public string Filename { get; set; }
        public string Version { get; set; }
        public string Nuget { get; set; }
    }

    public class ModuleManifestInformation
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public Version Version { get; set; }
        public string Website { get; set; }
        public ModuleManifestAuthor Author { get; set; }
    }

    public class ModuleManifestAuthor
    {
        public string Name { get; set; }
        public string Organization { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }

    public enum ModuleManifestOptionType
    {
        String, Boolean, Color, Number, Percentage, Timespan, Date
    }
}
