using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class ModuleFunctionAttribute : Attribute
    {
        // This is a positional argument
        public ModuleFunctionAttribute(ModuleFunctionType type)
        {
            Type = type;
        }

        public virtual ModuleFunctionType Type { get; internal set; }

        public bool Default { get; set; } = false;
        public string Identifier { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

    }

    public sealed class ActionFunctionAttribute : ModuleFunctionAttribute
    {
        public ActionFunctionAttribute() : base(ModuleFunctionType.Action) { }
    }

    public sealed class CheckFunctionAttribute: ModuleFunctionAttribute
    {
        public CheckFunctionAttribute(): base(ModuleFunctionType.Check) { }
    }

    public sealed class InfoFunctionAttribute : ModuleFunctionAttribute
    {
        public InfoFunctionAttribute() : base(ModuleFunctionType.Info) { }
    }
}
