namespace Hale.Lib.Modules.Manifest
{
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
}
