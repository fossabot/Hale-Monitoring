using System;
using Hale.Core.Data.Entities;
using Hale.Core.Data.Entities.Nodes;
using Hale.Core.Data.Entities.Users;

namespace Hale.Core.Model.Models
{
    public class NodeCommentDTO
    {
        public int Id { get; set; }
        public Node Node { get; set; }
        public Account User { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Text { get; set; }
    }
}
