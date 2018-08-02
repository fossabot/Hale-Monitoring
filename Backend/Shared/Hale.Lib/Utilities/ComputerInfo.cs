namespace Hale.Lib.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Text;
    using RI = System.Runtime.InteropServices.RuntimeInformation;

    public static partial class ComputerInfo
    {
        private const int GIGABYTE = 1024 * 1024 * 1024;

        private const string CpuInfoPath = "/proc/cpuinfo";
        private const string DmiBasePath = "/sys/devices/virtual/dmi/id";

        public static string GetOperatingSystemInfo()
        {
            return $"{RI.OSDescription.Trim()} {RI.OSArchitecture}";
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

#if NETSTANDARD2_0
        public static string GetHardwareInfo()
        {
            var sbHw = new StringBuilder();

            var vendor = GetDmiInfo("sys_vendor");
            var model = GetDmiInfo("product_name");

            if (TryGetCpus(out CpuInfo[] cpus))
            {
                sbHw.Append($"{cpus.Length}x({cpus[0].Threads}L/{cpus[0].Cores}P)@{cpus[0].ClockGhz:F2} GHz, ");
            }

            if (TryGetRam(out decimal total, out decimal _))
            {
                sbHw.Append($"{total / 1024 / 1024:F1} GiB");
            }

            /*
            var totalRam = ram.Select(r => long.Parse(r["Capacity"])).Sum();

            if (ram.Select(r => r["Capacity"]).Distinct().Count() == 1)
            {
                sbHw.Append($"({ram.Count}x{decimal.Divide(long.Parse(ram[0]["Capacity"]), GIGABYTE)}GB)");
            }
            */

            /*
             * sbHw.AppendLine($" @ {ram[0]["Speed"]}MHz");
             */

            return sbHw.ToString();
        }

        private static bool TryGetRam(out decimal total, out decimal free)
        {
            total = 0;
            free = 0;
            if (File.Exists(CpuInfoPath))
            {
                try
                {
                    foreach (var line in File.ReadAllLines(CpuInfoPath))
                    {
                        if (line.StartsWith("MemTotal"))
                        {
                            var parts = line.Split(' ');
                            if (parts.Length == 3 && decimal.TryParse(parts[1], out total) && free != 0)
                            {
                                return true;
                            }
                        }
                        else if (line.StartsWith("MemFree"))
                        {
                            var parts = line.Split(' ');
                            if (parts.Length == 3 && decimal.TryParse(parts[1], out free) && total != 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            return false;
        }

        private static bool TryGetCpus(out CpuInfo[] cpus)
        {
            // "Name", "MaxClockSpeed", "NumberOfCores", "NumberOfLogicalProcessors"
            var logCpus = new List<CpuInfo>();
            int phyCount = 0;

            if (File.Exists(CpuInfoPath))
            {
                try
                {
                    var cpu = default(CpuInfo);
                    foreach (var line in File.ReadAllLines(CpuInfoPath))
                    {
                        var parts = line.Split(':');
                        if (parts.Length < 2)
                        {
                            logCpus.Add(cpu);
                            cpu = default(CpuInfo);
                            continue;
                        }

                        var key = parts[0].Trim();
                        var value = parts[1].Trim();

                        switch (key)
                        {
                            case "model name":
                                cpu.Name = value;
                                break;

                            case "cpu MHz":
                                if (decimal.TryParse(value, out decimal clockDec))
                                {
                                    cpu.ClockGhz = clockDec / 1000;
                                }

                                break;

                            case "cpu cores":
                                if (int.TryParse(value, out int coreNum))
                                {
                                    cpu.Cores = coreNum;
                                }

                                break;

                            case "physical id":
                                if (int.TryParse(value, out int phyId))
                                {
                                    cpu.PhysicalID = phyId;
                                    phyCount = Math.Max(phyCount, phyId + 1);
                                }

                                break;
                        }
                    }

                    cpus = new CpuInfo[phyCount];

                    foreach (var lc in logCpus)
                    {
                        if (cpus[lc.PhysicalID].Threads == 0)
                        {
                            cpus[lc.PhysicalID] = lc;
                        }

                        cpus[lc.PhysicalID].Threads++;
                    }

                    return true;
                }
                catch
                {
                }
            }

            cpus = new CpuInfo[0];
            return false;
        }

        private static string GetDmiInfo(string id)
        {
            var dmiFile = Path.Combine(DmiBasePath, id);
            if (File.Exists(dmiFile))
            {
                try
                {
                    return File.ReadAllText(dmiFile);
                }
                catch
                {
                }
            }

            return null;
        }

#endif

        public struct CpuInfo
        {
            public string Name;
            public decimal ClockGhz;
            public int Cores;
            public int Threads;
            public int PhysicalID;
        }
    }
}
