namespace Hale.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;
    using Hale.Lib.Modules.Attributes;
    using Hale.Lib.Modules.Info;
    using Hale.Lib.Modules.Results;

    public partial class ServiceModule
    {
        [InfoFunction(Identifier = "list", Default = true)]
        public InfoResultSet ListServicesInfo(InfoSettings settings)
        {
            var ifr = new InfoResultSet();
            try
            {
                foreach (var service in settings.Targetless ? this.GetAllServices() : this.GetServiceDetails(settings.Targets))
                {
                    ifr.InfoResults.Add(service.Key, new InfoResult()
                    {
                        Items = service.Value,
                        RanSuccessfully = true,
                    });
                }

                ifr.RanSuccessfully = true;
                ifr.Message = $"Returned details for {ifr.InfoResults.Count} service(s).";

                if (!settings.Targetless)
                {
                    foreach (var target in settings.Targets)
                    {
                        if (!ifr.InfoResults.ContainsKey(target))
                        {
                            ifr.InfoResults.Add(target, new InfoResult()
                            {
                                Message = $"Could not retrieve information details for service \"{target}\"",
                                ExecutionException = new Exception("Target not found."),
                                RanSuccessfully = false
                            });
                        }
                    }
                }
            }
            catch (Exception x)
            {
                ifr.RanSuccessfully = false;
                ifr.FunctionException = x;
            }

            return ifr;
        }

        private Dictionary<string, Dictionary<string, string>> GetServiceDetails(ICollection<string> targets)
        {
            var services = new Dictionary<string, Dictionary<string, string>>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name,StartMode,StartName,Caption,State,Description FROM Win32_Service");

            foreach (ManagementObject mo in searcher.Get())
            {
                if (!targets.Contains(mo["Name"]))
                {
                    continue;
                }

                var properties = new Dictionary<string, string>();
                foreach (var p in mo.Properties)
                {
                    if (p.Name != "Name")
                    {
                        properties.Add(p.Name, p.Value != null ? p.Value.ToString() : string.Empty);
                    }
                }

                services.Add(mo["Name"].ToString(), properties);
            }

            return services;
        }

        private Dictionary<string, Dictionary<string, string>> GetAllServices()
        {
            var services = new Dictionary<string, Dictionary<string, string>>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name,StartMode,StartName,Caption,State FROM Win32_Service");

            foreach (ManagementObject mo in searcher.Get())
            {
                var properties = new Dictionary<string, string>();
                foreach (var p in mo.Properties)
                {
                    if (p.Name != "Name")
                    {
                        properties.Add(p.Name, p.Value != null ? p.Value.ToString() : string.Empty);
                    }
                }

                services.Add(mo["Name"].ToString(), properties);
            }

            return services;
        }
    }
}
