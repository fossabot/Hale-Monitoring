namespace Hale.Lib.Modules.Alerts
{
    using System;
    using Hale.Lib.Generalization;
    using Hale.Lib.Modules.Results;

    [Serializable]
    public class AlertResultRecord : ModuleResultRecord
    {
        private GenericValueDictionary<AlertResult> alertResults;

        public GenericValueDictionary<AlertResult> AlertResults
        {
            get
            {
                if (this.alertResults == null)
                {
                    this.alertResults = new GenericValueDictionary<AlertResult>(this.Results);
                }

                return this.alertResults;
            }
        }
    }
}
