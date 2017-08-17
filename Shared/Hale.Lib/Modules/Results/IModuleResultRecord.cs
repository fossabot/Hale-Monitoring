namespace Hale.Lib.Modules.Results
{
    using System;
    using System.Collections.Generic;

    public interface IModuleResultRecord
    {
        VersionedIdentifier Module { get; set; }

        VersionedIdentifier Runtime { get; set; }

        string Function { get; set; }

        ModuleFunctionType FunctionType { get; set; }

        Dictionary<string, object> Results { get; set; }

        DateTime CompletionTime { get; set; }
    }
}
