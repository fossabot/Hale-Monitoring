namespace Hale.Lib.Modules
{
    using System.Collections.Generic;

    public delegate ModuleFunctionResult ModuleFunction(ModuleSettingsBase settings);

    public interface IModuleProviderBase
    {
        Dictionary<string, ModuleFunction> Functions { get; set; }
    }
}
