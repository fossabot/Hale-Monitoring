using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Exceptions
{
    class UnknownUnsuccessfulException: Exception
    {
        public UnknownUnsuccessfulException(): 
            base("The function did not run successfully, but no exception was provided.")
        {
           
        }
    }
}
