using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Models.Modules
{
    public class CheckRecord
    {
        public int Id { get; set; }
        public Result Result { get; set; }
        public string Key { get; set; }
        public double Value { get; set; }
    }
}
