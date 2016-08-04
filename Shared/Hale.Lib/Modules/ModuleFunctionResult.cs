using Hale.Lib.Generalization;
using Hale.Lib.Modules.Actions;
using Hale.Lib.Modules.Alerts;
using Hale.Lib.Modules.Checks;
using Hale.Lib.Modules.Exceptions;
using Hale.Lib.Modules.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules
{
    [Serializable]
    public class ModuleFunctionResult
    {
        Exception _functionException;
        public Exception FunctionException {
            get
            {
                return (_functionException != null)
                    ? _functionException : new UnknownUnsuccessfulException();
            }
            set
            {
                _functionException = value;
            }
        }
        public string Message { get; set; }
        public bool RanSuccessfully { get; set; }
        public virtual Dictionary<string, object> Results { get; set; } = new Dictionary<string, object>();
        ICollection<string> Targets { get { return Results.Keys; } }
        public VersionedIdentifier Module { get; set; }
        public VersionedIdentifier Runtime { get; set; }
    }

    [Serializable]
    public class CheckFunctionResult: ModuleFunctionResult
    {
        GenericValueDictionary<CheckResult> _checkResults;
        public GenericValueDictionary<CheckResult> CheckResults
        {
            get
            {
                if (_checkResults == null)
                    _checkResults = new GenericValueDictionary<CheckResult>(Results);
                return _checkResults;
            }
        }
    }

    [Serializable]
    public class InfoFunctionResult : ModuleFunctionResult
    {
        GenericValueDictionary<InfoResult> _infoResults;
        public GenericValueDictionary<InfoResult> InfoResults
        {
            get
            {
                if (_infoResults == null)
                    _infoResults = new GenericValueDictionary<InfoResult>(Results);
                return _infoResults;
            }
        }
    }

    [Serializable]
    public class ActionFunctionResult : ModuleFunctionResult
    {
        GenericValueDictionary<ActionResult> _actionResults;
        public GenericValueDictionary<ActionResult> ActionResults {
            get {
                if (_actionResults == null)
                    _actionResults = new GenericValueDictionary<ActionResult>(Results);
                return _actionResults;
            }
        }
    }

    [Serializable]
    public class AlertFunctionResult : ModuleFunctionResult
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
