namespace Hale.Lib.Modules.Attributes
{
    using System;
    using Mode = TargetMode;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class ModuleFunctionAttribute : Attribute
    {
        // This is a positional argument
        public ModuleFunctionAttribute(ModuleFunctionType type)
        {
            this.Type = type;
        }

        public ModuleFunctionType Type { get; internal set; }

        public bool Default { get; set; } = false;

        public string Identifier { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Mode TargetMode { get; set; } = Mode.Targetless;
    }
}
