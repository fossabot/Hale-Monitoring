using Hale.Lib.Generalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Alerts
{
    [Serializable]
    public class AlertResult : ModuleResult
    {
    }

    [Serializable]
    public class AlertResultRecord : ModuleResultRecord
    {
        GenericValueDictionary<AlertResult> _alertResults;
        public GenericValueDictionary<AlertResult> AlertResults
        {
            get
            {
                if (_alertResults == null)
                    _alertResults = new GenericValueDictionary<AlertResult>(Results);
                return _alertResults;
            }
        }
    }
}
