using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Models.Modules
{
    public class InfoRecord
    {
        public int Id { get; set; }
        public virtual Result Result { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
