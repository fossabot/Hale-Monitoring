using Hale.Lib.Generalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Checks
{

    [Serializable]
    public class CheckResultRecord : ModuleResultRecord
    {
        GenericValueDictionary<CheckResult> _checkResults;
        public GenericValueDictionary<CheckResult> CheckResults
        {
            get
            {
                if (_checkResults == null && Results != null)
                    _checkResults = new GenericValueDictionary<CheckResult>(Results);
                return _checkResults;
            }
        }
    }

}
