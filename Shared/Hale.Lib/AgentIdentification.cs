using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Hale.Lib
{
    public class AgentIdentification
    {
        public string HardwareSummary { get; set; }
        public IdNetworkInterface[] NetworkInterfaces { get; set; }
        public string Hostname { get; set; }
        public string OperatingSystem { get; set; }
    }

    public class IdNetworkInterface
    {
        public string[] Addresses { get; set; }
        public string Name { get; set; }
        public string PhysicalAddress { get; set; } 
    }

}
