using System;
using Hale.Core.Models.Users;

namespace Hale.Core.Models.Nodes
{
    public class HostComment
    {
        public int Id { get; set; }
        public Host Node { get; set; }
        public Account User { get; set; }
        public DateTime Timestamp { get; set; }
        public string Text { get; set; }
    }
}