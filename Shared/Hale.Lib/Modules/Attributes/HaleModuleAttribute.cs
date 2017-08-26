#pragma warning disable CS3001 // Argument type is not CLS-compliant
#pragma warning disable CS3003 // Type is not CLS-compliant
namespace Hale.Lib.Modules.Attributes
{
    using System;
    using System.Linq;
    using Semver;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class HaleModuleAttribute : Attribute
    {
        public HaleModuleAttribute(string identifier, SemVersion version)
        {
            this.Identifier = identifier;
            this.Version = version;
        }

        public HaleModuleAttribute(string identifier, int major, int minor, int release)
            : this(identifier, new SemVersion(major, minor, release))
        {
        }

        public string Identifier { get; }

        public SemVersion Version { get; }

        public static HaleModuleAttribute GetFromType(Type type)
            => type.GetCustomAttributes(typeof(HaleModuleAttribute), inherit: false)
                .SingleOrDefault() as HaleModuleAttribute;
    }
}