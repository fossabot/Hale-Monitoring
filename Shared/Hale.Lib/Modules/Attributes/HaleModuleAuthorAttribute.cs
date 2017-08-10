using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class HaleModuleAuthorAttribute : Attribute
    {
        public HaleModuleAuthorAttribute(string author)
        {
            Author = author;
        }

        public string Author { get; }
        public string Organization { get; set; }
    }
}