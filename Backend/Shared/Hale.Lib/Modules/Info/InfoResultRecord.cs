namespace Hale.Lib.Modules.Info
{
    using System;
    using Hale.Lib.Generalization;
    using Hale.Lib.Modules.Results;

    [Serializable]
    public class InfoResultRecord : ModuleResultRecord
    {
        private GenericValueDictionary<InfoResult> infoResults;

        public GenericValueDictionary<InfoResult> InfoResults
            => this.infoResults != null
            ? this.infoResults
            : this.infoResults = new GenericValueDictionary<InfoResult>(this.Results);
    }
}
