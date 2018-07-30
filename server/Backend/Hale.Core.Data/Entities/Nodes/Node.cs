namespace Hale.Core.Data.Entities.Nodes
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public class Node
    {
        public int Id { get; set; }

        public string FriendlyName { get; set; }

        public string HostName { get; set; }

        public string Domain { get; set; }

        public string OperatingSystem { get; set; }

        public string NicSummary { get; set; }

        public string HardwareSummary { get; set; }

        public string Ip { get; set; }

        public Status Status { get; set; }

        public DateTimeOffset? LastConnected { get; set; }

        public DateTimeOffset? Modified { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

        public int? ConfiguredBy { get; set; }

        public Guid Guid { get; set; }

        [IgnoreDataMember]
        public byte[] RsaKey { get; set; }

        public List<NodeDetail> NodeDetails { get; set; }

        public bool Configured { get; set; }

        public bool Blocked { get; set; }
    }
}