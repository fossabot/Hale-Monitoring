using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class HaleModuleDescriptionAttribute : Attribute
    {
        public HaleModuleDescriptionAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; }
    }
}