using Hale.Lib.Modules.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules
{
    [Serializable]
    public class ModuleResult
    {
        Exception _executionException;
        public Exception ExecutionException
        {
            get
            {
                return (_executionException != null)
                    ? _executionException : new UnknownUnsuccessfulException();
            }
            set
            {
                _executionException = value;
            }
        }
        public bool RanSuccessfully { get; set; }
        public string Message { get; set; }
        public string Target { get; set; }
    }
}
