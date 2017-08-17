namespace Hale.Core.Model.Models
{
    using System;
    using System.Collections.Generic;
    using Hale.Core.Data.Entities.Nodes;
    using Hale.Core.Data.Entities.Users;

    public class NodeDTO
    {
        public bool Blocked { get; set; }

        public bool Configured { get; set; }

        public List<NodeDetail> NodeDetails { get; set; }

        public Guid Guid { get; set; }

        public Account ConfiguredBy { get; set; }

        public DateTimeOffset Created { get; set; }

        public Account ModifiedBy { get; set; }

        public DateTimeOffset? Modified { get; set; }

        public DateTimeOffset? LastConnected { get; set; }

        public Status Status { get; set; }

        public string Ip { get; set; }

        public string HardwareSummary { get; set; }

        public string NicSummary { get; set; }

        public string OperatingSystem { get; set; }

        public string Domain { get; set; }

        public string HostName { get; set; }

        public string FriendlyName { get; set; }

        public int Id { get; set; }
    }
}
