namespace Hale.Lib.Modules.Checks
{
    using System;
    using Hale.Lib.Modules.Results;

    [Serializable]
    public class CheckResult : ModuleResult
    {
        public CheckResult()
           : this("local")
        {
        }

        public CheckResult(string target)
        {
            this.Target = target;
        }

        public bool Warning { get; set; }

        public bool Critical { get; set; }

        public Datapoints RawValues { get; set; } = new Datapoints();

        public void SetThresholds(float value, CheckThresholds thresholds)
        {
            this.Warning = value > thresholds.Warning;
            this.Critical = value > thresholds.Critical;
        }
    }
}
