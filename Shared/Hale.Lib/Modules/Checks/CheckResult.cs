using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Checks
{
    [Serializable]
    public class CheckResult: ModuleResult
    {
        public bool Warning { get; set; }
        public bool Critical { get; set; }
        public Datapoints RawValues { get; set; } = new Datapoints();

        public void SetThresholds(float value, CheckThresholds thresholds)
        {
            Warning = value > thresholds.Warning;
            Critical = value > thresholds.Critical;
        }

        public CheckResult() : this("local") { }
        public CheckResult(string target)
        {
            Target = target;
        }
        
    }

}
