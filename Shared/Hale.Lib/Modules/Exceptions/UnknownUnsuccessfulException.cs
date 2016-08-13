using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Exceptions
{
    [Serializable]
    public class UnknownUnsuccessfulException: Exception
    {
        public UnknownUnsuccessfulException(): 
            base("The function did not run successfully, but no exception was provided.")
        {
           
        }

        public UnknownUnsuccessfulException(SerializationInfo si, StreamingContext sc): this()
        {
        }
    }
}
