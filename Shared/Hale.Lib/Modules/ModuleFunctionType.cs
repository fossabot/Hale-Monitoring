using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules
{
    public enum ModuleFunctionType: byte
    {
        None = 0,
        Check = 1,
        Info = 2,
        Action = 3,
        Alert = 4
    }
}
