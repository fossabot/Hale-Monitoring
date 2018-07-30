#pragma warning disable 108, 114

namespace Hale.Core.Data.Entities.Agent
{
    using System;
    using System.Collections.Generic;
    using Hale.Lib.Modules;
    using EModule = Hale.Core.Data.Entities.Modules.Module;

    public class AgentConfigSetFunction
    {
        public int Id { get; set; }

        public ModuleFunctionType Type { get; set; }

        public List<AgentConfigSetFunctionSettings> FunctionSettings { get; set; }

        public EModule Module { get; set; }
        public int ModuleId { get; set; }

        public string Function { get; set; } // TODO: Make this a reference to Function -NM 2017-08-10

        public TimeSpan Interval { get; set; }

        public bool Enabled { get; set; } = true;

        public bool Startup { get; set; } = false;

        public float WarningThreshold { get; set; }

        public float CriticalThreshold { get; set; }

        public AgentConfigSetCheckAction WarningAction { get; set; }

        public AgentConfigSetCheckAction CriticalAction { get; set; }

        public int AgentConfigSetId { get; set; }
        public AgentConfigSet AgentConfigSet { get; set; }
    }
}
