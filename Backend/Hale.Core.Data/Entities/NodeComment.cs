using System;

namespace Hale.Core.Data.Entities
{
    public class NodeComment
    {
        public int Id { get; set; }
        public Node Node { get; set; }
        public Account User { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
        public string Text { get; set; }
    }
}