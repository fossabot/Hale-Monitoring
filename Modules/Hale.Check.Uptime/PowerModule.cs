
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hale.Lib.Modules;
using Hale.Lib.Modules.Info;
using static Hale.Lib.Utilities.TimeSpanFormatter;
using Hale.Lib.Modules.Attributes;

namespace Hale.Modules
{

    [HaleModule("com.itshale.core.power", 0, 1, 1)]
    [HaleModuleName("Power Module")]
    public sealed class UptimeCheck: Module, IInfoProvider
    {

        public override string Name => "Power";
        public override string Author => "hale project";
        public override string Identifier => "com.itshale.core.power";
        public override string Platform => "Windows";
        public override decimal TargetApi => 1.2M;
        public override Version Version => new Version(0, 1, 1);

        Dictionary<string, ModuleFunction> IModuleProviderBase.Functions { get; set; }
            = new Dictionary<string, ModuleFunction>();

        [InfoFunction(Default = true, Identifier = "uptime")]
        [ReturnUnit("uptime", UnitType.Time, Precision = "second", Name = "Uptime in seconds")]
        public InfoResult DefaultInfo(InfoSettings settings)
        {
            var result = new InfoResult();

            try {
                TimeSpan uptime = new TimeSpan();

                float _raw;

                using (var utCounter = new System.Diagnostics.PerformanceCounter("System", "System Up Time"))
                {
                    utCounter.NextValue();
                    _raw = utCounter.NextValue();
                    uptime = TimeSpan.FromSeconds(_raw);
                }

                result.Items.Add("uptime", _raw.ToString());

                result.Message = "Uptime: " + HumanizeTimeSpan(uptime);

                result.RanSuccessfully = true;
            }
            catch (Exception x)
            {
                result.ExecutionException = x;
                result.RanSuccessfully = false;
            }

            return result;
        }

        public void InitializeInfoProvider(InfoSettings settings)
        {
            this.AddSingleResultInfoFunction(DefaultInfo);
            this.AddSingleResultInfoFunction("uptime", DefaultInfo);
        }

    }
}
