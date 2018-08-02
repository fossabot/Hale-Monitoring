#if NET471

namespace Hale.Lib.Utilities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;
    using System.Text;

    public static partial class ComputerInfo
    {
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
#endif