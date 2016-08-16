using Hale.Lib.Config;
using Hale.Lib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Models.Modules
{
    public class AgentConfigSet
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }

        public List<AgentConfigSetFuncSettings> Functions { get; set; }
        public List<AgentConfigSetTask> Tasks { get; set; }
    }

    public class AgentConfigSetTask: AgentConfigTask
    {
        public int Id { get; set; }
    }

    public class AgentConfigSetFuncSettings
    {
        public int Id { get; set; }

        public ModuleFunctionType Type;
        public List<AgentConfigSetFunctionSettings> FunctionSettings { get; set; }

        public Module Module { get; set; }
        public string Function { get; set; }
        public TimeSpan Interval { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Startup { get; set; } = false;

        public List<string> Targets { get; set; }

        public float WarningThreshold { get; set; }
        public float CriticalThreshold { get; set; }
        public AgentConfigSetCheckAction WarningAction { get; set; }
        public AgentConfigSetCheckAction CriticalAction { get; set; }
    }

    public class AgentConfigSetFunctionSettings
    {
        public int Id { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Target { get; set; }
    }

    [Serializable]
    public class AgentConfigSetCheckAction
    {
        public int Id { get; set; }

        public string Action { get; set; }
        public string Module { get; set; }
        public string Target { get; set; }
    }
}
