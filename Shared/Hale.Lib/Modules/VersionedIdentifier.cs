using Hale.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules
{
    [Serializable]
    public class VersionedIdentifier
    {
        public string Identifier { get; set; }
        public Version Version { get; set; }
        public override string ToString()
            => ToString(Identifier, Version);
        
        public static string ToString(Module module)
            => ToString(module.Identifier, module.Version);
        
        public static string ToString(string identifier, Version version)
            => $"{identifier}_v{version.Major}.{version.Minor}r{version.Revision}b{version.Build}";
        
        public VersionedIdentifier() { }
        public VersionedIdentifier(string identifier, Version version) { Version = version; Identifier = identifier; }
        public VersionedIdentifier(Module module) : this(module.Identifier, module.Version) { }

        internal static VersionedIdentifier Parse(string input)
        {
            string identifier, number;
            int major, minor, revision, build;
            bool success;

            (identifier, success, input) = input.EatUntil('_');
            if(!success) throw new FormatException("Invalid VersionedIdentifier format, expected \"_\" after identifier");

            (_, success, input) = input.EatChars(2);

            (number, success, input) = input.EatUntil('.');
            if (!success) throw new FormatException("Invalid VersionedIdentifier format, expected \".\" after major");
            if (!int.TryParse(number, out major)) throw new FormatException("Invalid VersionedIdentifier format, major is not a number");

            (_, success, input) = input.EatChars(1);

            (number, success, input) = input.EatUntil('r');
            if (!success) throw new FormatException("Invalid VersionedIdentifier format, expected \"r\" after minor");
            if (!int.TryParse(number, out minor)) throw new FormatException("Invalid VersionedIdentifier format, minor is not a number");

            (_, success, input) = input.EatChars(1);

            (number, success, input) = input.EatUntil('b');
            if (!success) throw new FormatException("Invalid VersionedIdentifier format, expected \"b\" after revision");
            if (!int.TryParse(number, out revision)) throw new FormatException("Invalid VersionedIdentifier format, revision is not a number");

            (_, success, number) = input.EatChars(1);

            if (!int.TryParse(number, out build)) throw new FormatException("Invalid VersionedIdentifier format, build is not a number");


            return new VersionedIdentifier(identifier, new Version(major, minor, build, revision));
    
        }
    }
}
