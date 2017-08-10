using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Attributes
{

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ReturnUnitAttribute : Attribute
    {
        public ReturnUnitAttribute(string identifier, UnitType unitType)
        {
            Identifier = identifier;
            Type = unitType;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public UnitType Type { get; set; }
        public string Precision { get; set; }

        public string Identifier { get; internal set; }

    }
}
