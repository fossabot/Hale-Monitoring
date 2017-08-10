using Hale.Core.Data.Entities.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Data.Entities
{
    public class NodeSummaryDTO
    {
        public int Id { get; set; }
        public string HostName { get; set; }
        public string FriendlyName { get; set; }
        public Status Status { get; set; }
        public DateTimeOffset? LastChange { get; set; }
    }
}
