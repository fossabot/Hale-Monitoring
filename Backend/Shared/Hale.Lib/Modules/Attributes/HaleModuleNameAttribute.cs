namespace Hale.Lib.Modules.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class HaleModuleNameAttribute : Attribute
    {
        public HaleModuleNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
