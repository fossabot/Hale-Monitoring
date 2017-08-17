namespace Hale.Lib.Modules.Results
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class ModuleResultRecord : IModuleResultRecord
    {
        public VersionedIdentifier Module { get; set; }

        public VersionedIdentifier Runtime { get; set; }

        public string Function { get; set; }

        public ModuleFunctionType FunctionType { get; set; }

        public virtual Dictionary<string, object> Results { get; set; }

        public DateTime CompletionTime { get; set; }

        private ICollection<string> Targets
        {
            get { return this.Results.Keys; }
        }
    }
}
