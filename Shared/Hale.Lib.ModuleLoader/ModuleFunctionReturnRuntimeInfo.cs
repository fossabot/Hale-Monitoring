using System;
using Hale.Lib.Modules;

namespace Hale.Lib.ModuleLoader
{
    [Serializable]
    public class ModuleFunctionReturnRuntimeInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public UnitType Type { get; set; }
        public string Precision { get; set; }
    }
}