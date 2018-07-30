namespace Hale.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Management;
    using System.Threading;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Attributes;
    using Hale.Lib.Modules.Checks;
    using Hale.Lib.Modules.Info;
    using Hale.Lib.Modules.Results;
    using Module = Hale.Lib.Modules.Module;

    [HaleModule("com.itshale.core.cpu", 0, 1, 1)]
    [HaleModuleName("CPU Module")]
    [HaleModuleDescription("CPU functions")]
    [HaleModuleAuthor("Hale Project")]
    public class CpuModule
    {
        [CheckFunction(Default = true, Identifier = "usage")]
        [ReturnUnit("CpuUsage", UnitType.Percent, Name = "CPU Usage", Description = "Percent of CPU time spent non-idle")]
        public CheckResult DefaultCheck(CheckSettings settings)
        {
            CheckResult cr = new CheckResult();

            try
            {
                PerformanceCounter cpuCounter;
                cpuCounter = new PerformanceCounter();

                cpuCounter.CategoryName = "Processor";
                cpuCounter.CounterName = "% Processor Time";
                cpuCounter.InstanceName = "_Total";

                int numSamples = settings.ContainsKey("samples") ? int.Parse(settings["samples"]) : 10;
                int sampleDelay = settings.ContainsKey("delay") ? int.Parse(settings["delay"]) : 200;

                float sampleSum = 0;
                float sampleMax = 0;
                float sampleMin = 100;

                cpuCounter.NextValue();

                for (int i = 0; i < numSamples; i++)
                {
                    float sample = cpuCounter.NextValue();

                    if (sample > sampleMax)
                    {
                        sampleMax = sample;
                    }

                    if (sample < sampleMin)
                    {
                        sampleMin = sample;
                    }

                    sampleSum += sample;
                    Thread.Sleep(sampleDelay + (i * 10));
                }

                sampleMax /= 100;
                sampleMin /= 100;

                float cpuPercentage = (sampleSum / numSamples) / 100;

                cr.RawValues.Add(new DataPoint("CpuUsage", cpuPercentage));

                cr.SetThresholds(cpuPercentage, settings.Thresholds);

                cr.Message = $"CPU usage average: {cpuPercentage.ToString("p1")}, min: {sampleMin.ToString("p1")}, max: {sampleMax.ToString("p1")}";

                cr.RanSuccessfully = true;
            }
            catch (Exception x)
            {
                cr.ExecutionException = x;
                cr.RanSuccessfully = false;
                cr.Message = "The check failed to execute due to exception: " + x.Message;
            }

            return cr;
        }

        [CheckFunction(Identifier = "performance", Name = "CPU Performance", Description="")]
        [ReturnUnit("CpuPerformance", UnitType.Percent, Name = "CPU Performance", Description = "How many percent of the CPUs maximum performance it's running at")]
        public CheckResult PerformanceCheck(CheckSettings settings)
        {
            CheckResult cr = new CheckResult();

            try
            {
                PerformanceCounter cpuCounter;
            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor Information";
            cpuCounter.CounterName = "% Processor Performance";
            cpuCounter.InstanceName = "_Total";

            int numSamples = settings.ContainsKey("samples") ? int.Parse(settings["samples"]) : 10;
            int sampleDelay = settings.ContainsKey("delay") ? int.Parse(settings["delay"]) : 200;

            float sampleSum = 0;
            float sampleMax = 0;
            float sampleMin = 100;

            cpuCounter.NextValue();

            for (int i = 0; i < numSamples; i++)
            {
                float sample = cpuCounter.NextValue();

                if (sample > sampleMax)
                    {
                        sampleMax = sample;
                    }

                    if (sample < sampleMin)
                    {
                        sampleMin = sample;
                    }

                    sampleSum += sample;
                Thread.Sleep(sampleDelay + (i * 10));
            }

                sampleMax /= 100;
                sampleMin /= 100;

                float cpuPercentage = (sampleSum / numSamples) / 100;

                cr.RawValues.Add(new DataPoint("CpuPerformance", cpuPercentage));

                cr.SetThresholds(cpuPercentage, settings.Thresholds);

                cr.Message = $"CPU performance average: {cpuPercentage.ToString("p1")}, min: {sampleMin.ToString("p1")}, max: {sampleMax.ToString("p1")}";

                cr.RanSuccessfully = true;
            }
            catch (Exception x)
            {
                cr.ExecutionException = x;
                cr.RanSuccessfully = false;
                cr.Message = "The check failed to execute due to exception: " + x.Message;
            }

            return cr;
        }

        [InfoFunction(Default = true)]
        public InfoResultSet DefaultInfo(InfoSettings settings)
        {
            var result = new InfoResultSet();
            try
            {
                var cpus = this.GetCPUProperties(new byte[] { }, new[] { "MaxClockSpeed", "NumberOfLogicalProcessors", "NumberOfCores", "Name", "Manufacturer" });
                foreach (var cpu in cpus)
                {
                    result.InfoResults.Add(cpu.Key.ToString(), new InfoResult()
                    {
                        RanSuccessfully = true,
                        Items = cpu.Value
                    });
                }

                result.Message = "Successfully retrieved default CPU info.";
                result.RanSuccessfully = true;
            }
            catch (Exception x)
            {
                result.FunctionException = x;
                result.Message = $"Cannot get info: {x.Message}.";
            }

            return result;
        }

        private Dictionary<byte, Dictionary<string, string>> GetCPUProperties(byte[] targets, string[] filter)
        {
            var cpus = new Dictionary<byte, Dictionary<string, string>>();

            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            var moc = searcher.Get();

            byte cpuId = 0;

            foreach (var mo in moc)
            {
                var items = new Dictionary<string, string>();
                foreach (var p in mo.Properties)
                {
                    if (p.Value != null && filter.Contains(p.Name))
                    {
                        items.Add(p.Name, p.Value.ToString().TrimEnd());
                    }
                }

                if (targets.Length == 0 || targets.Contains(cpuId))
                {
                    cpus.Add(cpuId, items);
                }

                cpuId++;
            }

            return cpus;
        }
    }
}
