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
        {
            return ToString(Identifier, Version);
        }
        public static string ToString(Module module)
        {
            return ToString(module.Identifier, module.Version);
        }
        public static string ToString(string identifier, Version version)
        {
            return $"{identifier}_v{version.Major}.{version.Minor}r{version.Revision}b{version.Build}";
        }
        public VersionedIdentifier() { }
        public VersionedIdentifier(string identifier, Version version) { Version = version; Identifier = identifier; }
        public VersionedIdentifier(Module module) : this(module.Identifier, module.Version) { }
    }
}
