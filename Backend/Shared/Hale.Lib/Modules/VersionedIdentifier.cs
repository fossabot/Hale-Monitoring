#pragma warning disable CS3001 // Argument type is not CLS-compliant
#pragma warning disable CS3003 // Type is not CLS-compliant
namespace Hale.Lib.Modules
{
    using System;
    using System.Runtime.Serialization;
    using Hale.Lib.Modules.Attributes;
    using Hale.Lib.Utilities;
    using Semver;

    [Serializable]
    public class VersionedIdentifier : ISerializable
    {
        public VersionedIdentifier()
        {
        }

        public VersionedIdentifier(string identifier, Version version)
            : this(identifier, new SemVersion(version))
        {
        }

        public VersionedIdentifier(string identifier, SemVersion version)
        {
            this.Version = version;
            this.Identifier = identifier;
        }

        public VersionedIdentifier(HaleModuleAttribute hma)
        {
            this.Version = hma.Version;
            this.Identifier = hma.Identifier;
        }

        public VersionedIdentifier(SerializationInfo info, StreamingContext context)
        {
            var version = info.GetValue("Version", typeof(string)) as string;

            if (version.Split('.').Length == 3)
            {
                this.Version = SemVersion.Parse(version);
            }
            else
            {
                this.Version = new SemVersion(System.Version.Parse(version));
            }

            this.Identifier = info.GetValue("Identifier", typeof(string)) as string;
        }

        public string Identifier { get; set; }

        public SemVersion Version { get; set; }

        public static VersionedIdentifier Parse(string input)
        {
            (string identifier, string version) = input.Split('_');

            // Remove the initial 'v'
            version = version.Substring(1);

            try
            {
                return new VersionedIdentifier(identifier, SemVersion.Parse(version));
            }
            catch (InvalidOperationException)
            {
                // If the version is not in the correct format, try the legacy format.
                // Throwing is expensive, but this case is much rarer and matching with eg. regex
                // adds unneccessary overhead                       -NM 2017-08-26
                return ParseLegacyFormat(identifier, version);
            }
        }

        public static VersionedIdentifier ParseLegacyFormat(string identifier, string version)
        {
            string number;
            bool success;

            (identifier, success, version) = version.EatUntil('_');
            if (!success)
            {
                throw new FormatException("Invalid VersionedIdentifier format, expected \"_\" after identifier");
            }

            (_, success, version) = version.EatChars(2);

            (number, success, version) = version.EatUntil('.');
            if (!success)
            {
                throw new FormatException("Invalid VersionedIdentifier format, expected \".\" after major");
            }

            if (!int.TryParse(number, out int major))
            {
                throw new FormatException("Invalid VersionedIdentifier format, major is not a number");
            }

            (_, success, version) = version.EatChars(1);

            (number, success, version) = version.EatUntil('r');
            if (!success)
            {
                throw new FormatException("Invalid VersionedIdentifier format, expected \"r\" after minor");
            }

            if (!int.TryParse(number, out int minor))
            {
                throw new FormatException("Invalid VersionedIdentifier format, minor is not a number");
            }

            (_, success, version) = version.EatChars(1);

            (number, success, version) = version.EatUntil('b');
            if (!success)
            {
                throw new FormatException("Invalid VersionedIdentifier format, expected \"b\" after revision");
            }

            if (!int.TryParse(number, out int revision))
            {
                throw new FormatException("Invalid VersionedIdentifier format, revision is not a number");
            }

            // NOTE: We don't actually need the build version, so we'll just skip parsing it -NM 2017-08-26
            /*
            (_, success, number) = version.EatChars(1);

            if (!int.TryParse(number, out int build))
            {
                throw new FormatException("Invalid VersionedIdentifier format, build is not a number");
            }
            */

            return new VersionedIdentifier(identifier, new Version(major, minor, revision));
        }

        public static string ToString(string identifier, SemVersion version)
            => $"{identifier}_v{version.Major}.{version.Minor}.{version.Patch}";

        public override string ToString()
            => ToString(this.Identifier, this.Version);

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Identifier", this.Identifier, typeof(string));
            info.AddValue("Version", this.Version.ToString(), typeof(string));
        }
    }
}
