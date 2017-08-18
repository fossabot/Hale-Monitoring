namespace Hale.Lib.Modules.Results
{
    using System;
    using Hale.Lib.Generalization;
    using Hale.Lib.Modules.Checks;

    [Serializable]
    public class CheckResultSet : ModuleResultSet
    {
        private GenericValueDictionary<CheckResult> checkResults;

        public GenericValueDictionary<CheckResult> CheckResults
            => this.checkResults != null
            ? this.checkResults
            : this.checkResults = new GenericValueDictionary<CheckResult>(this.Results);
    }
}
