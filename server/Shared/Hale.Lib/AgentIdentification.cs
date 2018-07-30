namespace Hale.Lib
{
    public class AgentIdentification
    {
        public string HardwareSummary { get; set; }

        public IdNetworkInterface[] NetworkInterfaces { get; set; }

        public string Hostname { get; set; }

        public string OperatingSystem { get; set; }
    }
}
