namespace Hale.Checks
{
    using System;
    using System.Diagnostics;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Attributes;
    using Hale.Lib.Modules.Checks;

    [HaleModule("com.itshale.core.paging", 0, 1, 1)]
    [HaleModuleName("Paging Usage Module")]
    [HaleModuleAuthor("Hale Project")]
    public class PagingUsage
    {
        [CheckFunction(Default = true, Identifier ="paging")]
        [ReturnUnit("paging", UnitType.Percent, Precision = "percent", Name ="% Usage")]
        public CheckResult DefaultCheck(CheckSettings settings)
        {
            CheckResult result = new CheckResult();

            try
            {
                PerformanceCounter pagefileUsagePercent = new PerformanceCounter()
                {
                    CounterName = "% Usage",
                    CategoryName = "Paging File",
                    InstanceName = "_Total"
                };

                pagefileUsagePercent.NextValue();

                float usagePercent = pagefileUsagePercent.NextValue();

                result.Message = $"Paging File Usage: {usagePercent}% of total";
                result.RawValues.Add(new DataPoint() { DataType = "usagePercent", Value = usagePercent });
                result.RanSuccessfully = true;
            }
            catch (Exception e)
            {
                result.Message = "Could not fetch swap usage information.";
                result.RanSuccessfully = false;
                result.ExecutionException = e;
            }

            return result;
        }
    }
}
