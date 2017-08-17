namespace Hale.Lib.ModuleLoader
{
    using System;
    using Hale.Lib.Modules;

    [Serializable]
    public class ModuleFunctionReturnRuntimeInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public UnitType Type { get; set; }

        public string Precision { get; set; }
    }
}