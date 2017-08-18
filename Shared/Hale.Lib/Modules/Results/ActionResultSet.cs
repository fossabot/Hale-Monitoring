namespace Hale.Lib.Modules.Results
{
    using System;
    using Hale.Lib.Generalization;
    using Hale.Lib.Modules.Actions;

    [Serializable]
    public class ActionResultSet : ModuleResultSet
    {
        private GenericValueDictionary<ActionResult> actionResults;

        public GenericValueDictionary<ActionResult> ActionResults
            => this.actionResults != null
            ? this.actionResults
            : this.actionResults = new GenericValueDictionary<ActionResult>(this.Results);
    }
}
