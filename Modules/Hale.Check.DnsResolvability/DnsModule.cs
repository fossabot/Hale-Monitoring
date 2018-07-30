namespace Hale.Checks
{
    using System;
    using System.Net;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Attributes;
    using Hale.Lib.Modules.Checks;

    /// <summary>
    /// All checks need to realize the interface ICheck.
    /// </summary>
    [HaleModule("com.itshale.core.dns", 0, 1, 1)]
    [HaleModuleName("DNS Module")]
    [HaleModuleAuthor("Hale Project")]
    public sealed class DnsModule
    {
        [CheckFunction(Default = true, Identifier = "resolve", TargetMode = TargetMode.Multiple)]
        [ReturnUnit("resolvedPercent", UnitType.Percent, Name = "% Resolved")]
        public CheckResult DefaultCheck(CheckSettings settings)
        {
            if (settings.Targetless)
            {
                return new CheckResult()
                {
                    Message = $"No targets where configured.",
                    RanSuccessfully = false
                };
            }

            CheckResult result = new CheckResult();
            int failed = 0;

            foreach (string target in settings.Targets)
             {
                try
                {
                    IPAddress[] addresses = Dns.GetHostAddresses(target);
                    if (addresses.Length == 0)
                    {
                        failed++;
                    }

                    result.RanSuccessfully = true;
                }
                catch (Exception)
                {
                    failed++;
                }
            }

            if (failed == settings.Targets.Count)
            {
                result.RanSuccessfully = false;
            }

            int successfulPercentage = failed / settings.Targets.Count;

            result.Message = $"{successfulPercentage}% of " + settings.Targets.Count + " DNS Resolves was succesful.";
            result.RawValues.Add(new DataPoint() { DataType = "successfulPercentage", Value = successfulPercentage });
            result.RanSuccessfully = true;

            return result;
        }
    }
}
