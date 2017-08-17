namespace Hale.Core.Data.Entities
{
    using System;
    using Hale.Core.Data.Entities.Nodes;

    public class NodeSummaryDTO
    {
        public int Id { get; set; }

        public string HostName { get; set; }

        public string FriendlyName { get; set; }

        public Status Status { get; set; }

        public DateTimeOffset? LastChange { get; set; }
    }
}
