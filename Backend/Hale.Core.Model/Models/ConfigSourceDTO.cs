using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Models
{
    /// <summary>
    /// A YAML representation of a node config source paired with an ID
    /// </summary>
    public class ConfigSourceDTO
    {
        public int Id { get; set; }
        public string Body { get; set; }
    }
}
