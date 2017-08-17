namespace Hale.Lib.Modules.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class HaleModuleAttribute : Attribute
    {
        public HaleModuleAttribute(string identifier, Version version)
        {
            this.Identifier = identifier;
            this.Version = version;
        }

        public HaleModuleAttribute(string identifier, int major, int minor, int release)
            : this(identifier, new Version(major, minor, 0, release))
        {
        }

        public string Identifier { get; }

        public Version Version { get; }
    }
}
