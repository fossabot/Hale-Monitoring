namespace Hale.Checks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Checks;
    using Hale.Lib.Modules.Info;

    /// <summary>
    /// All checks need to realize the interface ICheck.
    /// </summary>
    public class PagingUsage : Module, ICheckProvider, IInfoProvider
    {
        public override string Name => "Page File";

        public override string Author => "Hale Project";

        public override string Identifier => "com.itshale.core.paging";

        public override Version Version => new Version(0, 1, 1);

        public override string Platform => "Windows";

        public override decimal TargetApi => 1.2M;

        Dictionary<string, ModuleFunction> IModuleProviderBase.Functions { get; set; }
            = new Dictionary<string, ModuleFunction>();

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

        public InfoResult DefaultInfo(InfoSettings settings)
        {
            var result = new InfoResult();
            result.ExecutionException = new NotImplementedException();
            return result;
        }

        public void InitializeCheckProvider(CheckSettings settings)
        {
            this.AddSingleResultCheckFunction(this.DefaultCheck);
            this.AddSingleResultCheckFunction("usage", this.DefaultCheck);
        }

        public void InitializeInfoProvider(InfoSettings settings)
        {
            this.AddSingleResultInfoFunction(this.DefaultInfo);
            this.AddSingleResultInfoFunction("sizes", this.DefaultInfo);
        }
    }
}
