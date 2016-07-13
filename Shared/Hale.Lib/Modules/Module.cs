using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules
{
    public abstract class Module
    {
        /// <summary>
        /// Module name, if not overriden it is extracted from the assembly FileDescription
        /// </summary>
        public virtual string Name
        {
            get
            {
                var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                return fvi.FileDescription;
            }
        }

        /// <summary>
        /// Module author, if not overriden it is extracted from the assembly CompanyName
        /// </summary>
        public virtual string Author
        {
            get
            {
                var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                return fvi.CompanyName;
            }
        }

        /// <summary>
        /// Module version, returns assembly version if not overridden
        /// </summary>
        public virtual Version Version {
            get {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        /// <summary>
        /// Module target platform
        /// </summary>
        public abstract string Platform { get; }

        /// <summary>
        /// The FQN (Fully Qualified Name) identifier for the module, ie.: com.organization.package.module
        /// </summary>
        public abstract string Identifier { get; }

        /// <summary>
        /// Module target API, should match Hale-Lib DLL version
        /// </summary>
        public virtual Decimal TargetApi {
            get
            {
                var haleLibAssembly = Assembly.GetExecutingAssembly().GetReferencedAssemblies().First((an) => { return an.Name == "Hale-Lib"; });
                return haleLibAssembly.Version.Major + (haleLibAssembly.Version.Minor * 0.1M);
            }
        }
    }
}
