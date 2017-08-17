namespace Hale.Lib.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public abstract class Module : IModuleProviderBase
    {
        /// <summary>
        /// Module name, if not overriden it is extracted from the assembly FileDescription
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Module author, if not overriden it is extracted from the assembly CompanyName
        /// </summary>
        public virtual string Author => string.Empty;

        /// <summary>
        /// Module version, returns assembly version if not overridden
        /// </summary>
        public abstract Version Version { get; }

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
        public virtual decimal TargetApi
        {
            get
            {
                var haleLibAssembly = Assembly.GetEntryAssembly().GetReferencedAssemblies().First((an) => { return an.Name == "Hale-Lib"; });
                return haleLibAssembly.Version.Major + (haleLibAssembly.Version.Minor * 0.1M);
            }
        }

        public Dictionary<string, ModuleFunction> Functions { get; set; } = new Dictionary<string, ModuleFunction>();
    }
}
