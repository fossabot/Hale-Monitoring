namespace Hale.Core.Models
{
    using System;
    using Hale.Core.Model.Models;

    /// <summary>
    /// A summary of a node config
    /// </summary>
    public class ConfigSummaryDTO
    {
        public int Id { get; set; }

        public string Identifier { get; set; }

        public int CheckCount { get; set; }

        public int ActionCount { get; set; }

        public int InfoCount { get; set; }

        public DateTimeOffset? Modified { get; set; }

        public UserBasicsDTO ModifiedBy { get; set; }

        public DateTimeOffset Created { get; set; }

        public UserBasicsDTO CreatedBy { get; set; }
    }
}
