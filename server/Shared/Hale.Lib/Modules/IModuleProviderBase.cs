namespace Hale.Lib.Modules
{
    using System.Collections.Generic;

    public delegate ModuleResultSet ModuleFunction(ModuleSettingsBase settings);

    public interface IModuleProviderBase
    {
        Dictionary<string, ModuleFunction> Functions { get; set; }
    }
}
