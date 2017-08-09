using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class HaleModuleAttribute : Attribute
    {
        // This is a positional argument
        public HaleModuleAttribute(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; internal set; }
    }
}
