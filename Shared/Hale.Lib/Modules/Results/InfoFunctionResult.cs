namespace Hale.Lib.Modules.Results
{
    using System;
    using Hale.Lib.Generalization;
    using Hale.Lib.Modules.Info;

    [Serializable]
    public class InfoFunctionResult : ModuleFunctionResult
    {
        private GenericValueDictionary<InfoResult> infoResults;

        public GenericValueDictionary<InfoResult> InfoResults
            => this.infoResults != null
            ? this.infoResults
            : this.infoResults = new GenericValueDictionary<InfoResult>(this.Results);
    }
}
