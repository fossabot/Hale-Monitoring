namespace Hale.Core.Model.Models
{
    using System;
    using Hale.Core.Data.Entities.Nodes;
    using Hale.Core.Data.Entities.Users;

    public class NodeCommentDTO
    {
        public int Id { get; set; }

        public Node Node { get; set; }

        public Account User { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string Text { get; set; }
    }
}
