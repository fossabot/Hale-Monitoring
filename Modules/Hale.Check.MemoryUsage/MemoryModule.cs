namespace Hale.Checks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Attributes;
    using Hale.Lib.Modules.Checks;
    using Hale.Lib.Modules.Info;
    using static Hale.Lib.Utilities.StorageUnitFormatter;

    /// <summary>
    /// All checks need to realize the interface ICheck.
    /// </summary>
    [HaleModule("com.itshale.core.memory", 0, 1, 1)]
    public class MemoryModule : Module, ICheckProvider, IInfoProvider
    {
        public override string Name => "Memory Module";

        public override string Author => "Hale Project";

        public override string Identifier => "com.itshale.core.memory";

        public override Version Version => new Version(0, 1, 1);

        public override string Platform => "Windows";

        public override decimal TargetApi => 1.2M;

        [CheckFunction(Default = true, Identifier = "usage")]
        [ReturnUnit("freePercentage", UnitType.Percent, Name = "Free Relative")]
        [ReturnUnit("freeBytes", UnitType.StorageUnit, Precision = "byte", Name = "Free Absolute")]
        public CheckResult DefaultCheck(CheckSettings settings)
        {
            CheckResult result = new CheckResult();

            try
            {
                PerformanceCounter ramPercentage = new PerformanceCounter()
                {
                    CounterName = "% Committed Bytes in Use",
                    CategoryName = "Memory"
                };

                ramPercentage.NextValue();

                float freePercentage = 1.0F - (ramPercentage.NextValue() / 100.0F);

                var ci = new Microsoft.VisualBasic.Devices.ComputerInfo();

                ulong memoryTotal = ci.TotalPhysicalMemory;

                // Note: ci.AvailablePhysicalMemory does not return "accurate" data -NM
                // ulong memoryFree = ci.AvailablePhysicalMemory;

                // Hack: Using this calculated approximation for now. -NM
                ulong memoryFree = (ulong)Math.Round(memoryTotal * freePercentage);

                result.Message = $"RAM Usage: {HumanizeStorageUnit(memoryFree)}free of total {HumanizeStorageUnit(memoryTotal)}({freePercentage.ToString("P1")})";

                // Raw value is percent of free RAM (0.0 .. 1.0)
                result.RawValues.Add(new DataPoint() { DataType = "freePercentage", Value = freePercentage });
                result.RawValues.Add(new DataPoint() { DataType = "freeBytes", Value = (float)memoryFree });

                result.SetThresholds(freePercentage, settings.Thresholds);

                result.RanSuccessfully = true;
            }
            catch (Exception e)
            {
                result.Message = "Could not get RAM information.";
                result.RanSuccessfully = false;
                result.ExecutionException = e;
            }

            return result;
        }

        [InfoFunction(Default = true, Identifier = "sizes")]
        [ReturnUnit("size", UnitType.StorageUnit, Precision = "byte", Name = "Memory Size")]
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
