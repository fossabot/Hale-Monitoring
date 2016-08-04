using Hale.Lib.Generalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Actions
{
    [Serializable]
    public class ActionResult: ModuleResult
    {

    }

    [Serializable]
    public class ActionResultRecord : ModuleResultRecord
    {
        GenericValueDictionary<ActionResult> _actionResults;
        public GenericValueDictionary<ActionResult> ActionResults
        {
            get
            {
                if (_actionResults == null)
                    _actionResults = new GenericValueDictionary<ActionResult>(Results);
                return _actionResults;
            }
        }
    }
}
