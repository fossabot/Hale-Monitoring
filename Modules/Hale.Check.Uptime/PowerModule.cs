namespace Hale.Modules
{
    using System;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Attributes;
    using Hale.Lib.Modules.Info;
    using static Hale.Lib.Utilities.TimeSpanFormatter;

    [HaleModule("com.itshale.core.power", 0, 1, 1)]
    [HaleModuleName("Power Module")]
    [HaleModuleAuthor("Hale Project")]
    public sealed class PowerModule
    {
        [InfoFunction(Default = true, Identifier = "uptime")]
        [ReturnUnit("uptime", UnitType.Time, Precision = "second", Name = "Uptime in seconds")]
        public InfoResult DefaultInfo(InfoSettings settings)
        {
            var result = new InfoResult();

            try
            {
                TimeSpan uptime = default(TimeSpan);
                float raw;

                using (var utCounter = new System.Diagnostics.PerformanceCounter("System", "System Up Time"))
                {
                    utCounter.NextValue();
                    raw = utCounter.NextValue();
                    uptime = TimeSpan.FromSeconds(raw);
                }

                result.Items.Add("uptime", raw.ToString());
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
    }
}
