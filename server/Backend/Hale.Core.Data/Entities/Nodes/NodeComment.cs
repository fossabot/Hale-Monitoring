namespace Hale.Core.Data.Entities.Nodes
{
    using System;
    using Hale.Core.Data.Entities.Users;

    public class NodeComment
    {
        public int Id { get; set; }

        public Node Node { get; set; }

        public Account User { get; set; }

        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;

        public string Text { get; set; }
    }
}