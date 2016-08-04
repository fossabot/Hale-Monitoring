using Hale.Lib.Generalization;
using Hale.Lib.Modules.Actions;
using Hale.Lib.Modules.Checks;
using Hale.Lib.Modules.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules
{
    public interface IModuleResultRecord
    {
        VersionedIdentifier Module { get; set; }
        VersionedIdentifier Runtime { get; set; }
        string Function { get; set; }
        ModuleFunctionType FunctionType { get; set; }
        Dictionary<string, object> Results { get; set; }
        DateTime CompletionTime { get; set; }
    }

    [Serializable]
    public class ModuleResultRecord: IModuleResultRecord
    {
        public VersionedIdentifier Module { get; set; }
        public VersionedIdentifier Runtime { get; set; }
        public string Function { get; set; }
        public ModuleFunctionType FunctionType { get; set; }
        public virtual Dictionary<string, object> Results { get; set; }
        public DateTime CompletionTime { get; set; }
        ICollection<string> Targets { get { return Results.Keys; } }
    }



    [Serializable]
    public class ResultRecordChunk : Dictionary<Guid, object>
    {

    }

}
