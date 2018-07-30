namespace Hale.Lib.Modules.Checks
{
    using System;
    using Hale.Lib.Generalization;
    using Hale.Lib.Modules.Results;

    [Serializable]
    public class CheckResultRecord : ModuleResultRecord
    {
        private GenericValueDictionary<CheckResult> checkResults;

        public GenericValueDictionary<CheckResult> CheckResults
        {
            get
            {
                if (this.checkResults == null && this.Results != null)
                {
                    this.checkResults = new GenericValueDictionary<CheckResult>(this.Results);
                }

                return this.checkResults;
            }
        }
    }
}
