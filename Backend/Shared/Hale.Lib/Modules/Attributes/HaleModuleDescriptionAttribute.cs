namespace Hale.Lib.Modules.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class HaleModuleDescriptionAttribute : Attribute
    {
        public HaleModuleDescriptionAttribute(string description)
        {
            this.Description = description;
        }

        public string Description { get; }
    }
}