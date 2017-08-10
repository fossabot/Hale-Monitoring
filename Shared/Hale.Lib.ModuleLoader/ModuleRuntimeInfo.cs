using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hale.Lib.Modules;

namespace Hale.Lib.ModuleLoader
{
    [Serializable]
    public class ModuleRuntimeInfo
    {
        public Dictionary<ModuleFunctionType, Dictionary<string, ModuleFunctionRuntimeInfo>> Functions { get; set; }

        public VersionedIdentifier Module { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Organization { get; set; }

        public static ModuleRuntimeInfo Empty => new ModuleRuntimeInfo()
        {
            Functions = new Dictionary<ModuleFunctionType, Dictionary<string, ModuleFunctionRuntimeInfo>>()
            {
                { ModuleFunctionType.Check, new Dictionary<string, ModuleFunctionRuntimeInfo>() },
                { ModuleFunctionType.Info, new Dictionary<string, ModuleFunctionRuntimeInfo>() },
                { ModuleFunctionType.Action, new Dictionary<string, ModuleFunctionRuntimeInfo>() },
                { ModuleFunctionType.Alert, new Dictionary<string, ModuleFunctionRuntimeInfo>() },
            }
        };
    }
}
