namespace Hale.Lib.Utilities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;
    using System.Net.NetworkInformation;
    using System.Text;

    public static class ComputerInfo
    {
        private const int GIGABYTE = 1024 * 1024 * 1024;

        public static string GetOperatingSystemInfo()
        {
            var osi = GetMultipleProperties("SELECT * FROM Win32_OperatingSystem", new[] { "Caption", "OSArchitecture", "Version" }).FirstOrDefault();
            if (osi == null)
            {
                return "Unknown";
            }

            return $"{osi["Caption"]} {osi["OSArchitecture"]} {osi["Version"]}";
        }

        public static string GetHardwareInfo()
        {
            var sbHw = new StringBuilder();

            var sys = GetMultipleProperties("SELECT * FROM Win32_ComputerSystem", new[] { "Manufacturer", "Model" }).FirstOrDefault();

            if (sys != null || sys["Manufacturer"].Contains("O.E.M."))
            {
                sbHw.Append("Custom, ");
            }
            else
            {
                sbHw.Append($"{sys["Manufacturer"]} {sys["Model"]}, ");
            }

            var cpus = GetMultipleProperties("SELECT * FROM Win32_Processor", new[] { "Name", "MaxClockSpeed", "NumberOfCores", "NumberOfLogicalProcessors" });

            string cpuSpeed = (decimal.Parse(cpus[0]["MaxClockSpeed"]) / 1000M).ToString("F2");

            // sbHw.Append($"CPU: {cpus.Count}x{cpus[0]["Name"].Trim()}");
            sbHw.Append($"{cpus.Count}x({cpus[0]["NumberOfLogicalProcessors"]}L/{cpus[0]["NumberOfCores"]}P)@{cpuSpeed}GHz, ");

            var ram = GetMultipleProperties("SELECT * FROM Win32_PhysicalMemory", new[] { "Capacity", "Speed" });

            var totalRam = ram.Select(r => long.Parse(r["Capacity"])).Sum();
            sbHw.Append($"{decimal.Divide(totalRam, GIGABYTE).ToString("F1")}GB");

            if (ram.Select(r => r["Capacity"]).Distinct().Count() == 1)
            {
                sbHw.Append($"({ram.Count}x{decimal.Divide(long.Parse(ram[0]["Capacity"]), GIGABYTE)}GB)");
            }

            /*
             * sbHw.AppendLine($" @ {ram[0]["Speed"]}MHz");
             */

            return sbHw.ToString();
        }

        public static IdNetworkInterface[] GetNetworkInterfaceInfo()
        {
            var inis = new List<IdNetworkInterface>();
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var nic in nics.Where(nic =>
                nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
            {
                var ini = new IdNetworkInterface();

                var ips = new List<string>();
                foreach (var ipi in nic.GetIPProperties().UnicastAddresses)
                {
                    ips.Add(ipi.Address.ToString());
                }

                ini.Addresses = ips.ToArray();

                ini.Name = nic.Description;
                ini.PhysicalAddress = string
                    .Join("-", nic.GetPhysicalAddress().GetAddressBytes()
                    .Select(b => b.ToString("X2")));

                inis.Add(ini);
            }

            return inis.ToArray();
        }

        private static List<Dictionary<string, string>> GetMultipleProperties(string query, string[] filter)
        {
            var instances = new List<Dictionary<string, string>>();

            var searcher = new ManagementObjectSearcher(query);
            var moc = searcher.Get();

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

                instances.Add(items);
            }

            return instances;
        }
    }
}
