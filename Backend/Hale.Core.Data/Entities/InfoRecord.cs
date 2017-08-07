using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Data.Entities
{
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public class InfoRecord
    {
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public virtual Result Result { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Value { get; set; }
    }
}
