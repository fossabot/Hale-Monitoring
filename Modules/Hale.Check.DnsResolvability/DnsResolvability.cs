using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Hale.Lib;
using Hale.Lib.Modules;
using Hale.Lib.Modules.Checks;
using Hale.Lib.Modules.Info;

using static Hale.Lib.Utilities.StorageUnitFormatter;


namespace Hale.Checks
{
    /// <summary>
    /// All checks need to realize the interface ICheck.
    /// </summary>
    public sealed class DnsResolvability: Module, ICheckProvider, IInfoProvider
    {

        public override string Name => "DNS Resolvability Test";
        public override string Author => "Hale Project";
        public override string Identifier => "com.itshale.core.paging";
        public override Version Version => new Version (0, 1, 1);
        public override string Platform => "Windows";
        public override decimal TargetApi => 1.2M;

        Dictionary<string, ModuleFunction> IModuleProviderBase.Functions { get; set; }
            = new Dictionary<string, ModuleFunction>();


        public CheckResult DefaultCheck(CheckSettings settings)
        {
            
            if (settings.Targetless) { 
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
                        failed++;
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

            int successfulPercentage = (failed / settings.Targets.Count);

            result.Message = $"{successfulPercentage}% of " + settings.Targets.Count + " DNS Resolves was succesful.";
            result.RawValues.Add(new DataPoint() { DataType = "successfulPercentage", Value = successfulPercentage});
            result.RanSuccessfully = true;

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
            this.AddSingleResultCheckFunction(DefaultCheck);
            this.AddSingleResultCheckFunction("usage", DefaultCheck);
        }

        public void InitializeInfoProvider(InfoSettings settings)
        {
            this.AddSingleResultInfoFunction(DefaultInfo);
            this.AddSingleResultInfoFunction("sizes", DefaultInfo);
        }

    }
}
