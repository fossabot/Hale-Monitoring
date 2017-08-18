namespace Hale.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;
    using System.ServiceProcess;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Attributes;
    using Hale.Lib.Modules.Checks;
    using Hale.Lib.Modules.Results;

    public partial class ServiceModule
    {
        [CheckFunction(Default = true, Identifier = "running")]
        [ReturnUnit("status", UnitType.Custom, Name = "Status Code", Description = "Service status code")]
        public CheckResultSet ServiceRunningCheck(CheckSettings settings)
        {
            var cfr = new CheckResultSet();

            try
            {
                IEnumerable<string> services = settings.Targetless ? this.GetAutomaticServices() : settings.Targets;
                foreach (var service in services)
                {
                    var cr = new CheckResult();
                    try
                    {
                        ServiceController sc = new ServiceController(service);

                        // Set warning if the service is not running
                        cr.Warning = sc.Status != ServiceControllerStatus.Running;

                        // Set critical if the service is either stopping or stopped
                        cr.Critical = CriticalStatuses.Contains(sc.Status);

                        cr.Message = $"Service \"{sc.DisplayName}\" has the status of {sc.Status.ToString()}.";

                        cr.RawValues.Add(new DataPoint("status", (int)sc.Status));

                        cr.RanSuccessfully = true;
                    }
                    catch (Exception x)
                    {
                        cr.ExecutionException = x;
                        cr.RanSuccessfully = false;
                    }

                    cfr.CheckResults.Add(service, cr);
                }

                cfr.RanSuccessfully = true;
            }
            catch (Exception x)
            {
                cfr.FunctionException = x;
                cfr.Message = x.Message;
                cfr.RanSuccessfully = false;
            }

            return cfr;
        }

        private string[] GetAutomaticServices()
        {
            var services = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Service WHERE StartMode = 'Auto'");

            foreach (ManagementObject mo in searcher.Get())
            {
                services.Add(mo["Name"].ToString());
            }

            return services.ToArray();
        }
    }
}
