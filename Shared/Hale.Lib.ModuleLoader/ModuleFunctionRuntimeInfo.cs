using System;
using System.Collections.Generic;

namespace Hale.Lib.ModuleLoader
{
    [Serializable]
    public class ModuleFunctionRuntimeInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, ModuleFunctionReturnRuntimeInfo> Returns { get; set; }
            = new Dictionary<string, ModuleFunctionReturnRuntimeInfo>();
    }
}