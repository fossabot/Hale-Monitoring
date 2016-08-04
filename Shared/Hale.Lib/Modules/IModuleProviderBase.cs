using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules
{
    public interface IModuleProviderBase 
    {
        Dictionary<string, ModuleFunction> Functions { get; set; }
    }

    public delegate ModuleFunctionResult ModuleFunction(ModuleSettingsBase settings);
}
