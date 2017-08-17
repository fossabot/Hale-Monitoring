namespace Hale.Lib.Modules.Actions
{
    using System;
    using Hale.Lib.Generalization;
    using Hale.Lib.Modules.Results;

    [Serializable]
    public class ActionResultRecord : ModuleResultRecord
    {
        private GenericValueDictionary<ActionResult> actionResults;

        public GenericValueDictionary<ActionResult> ActionResults
            => this.actionResults != null
            ? this.actionResults
            : this.actionResults = new GenericValueDictionary<ActionResult>(this.Results);
    }
}
