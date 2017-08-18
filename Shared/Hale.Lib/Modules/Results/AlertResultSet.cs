namespace Hale.Lib.Modules.Results
{
    using System;
    using Hale.Lib.Generalization;
    using Hale.Lib.Modules.Alerts;

    [Serializable]
    public class AlertResultSet : ModuleResultSet
    {
        private GenericValueDictionary<AlertResult> alertResults;

        public GenericValueDictionary<AlertResult> AlertResults
            => this.alertResults != null
            ? this.alertResults
            : this.alertResults = new GenericValueDictionary<AlertResult>(this.Results);
    }
}
