using System;
using System.Collections.Generic;
using Hale.Lib.Config;
using Hale.Lib.Modules;
using Module = Hale.Core.Models.Modules.Module;
#pragma warning disable 108,114

namespace Hale.Core.Models.Agent
{
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public class AgentConfigSet
    {
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int? CreatedBy { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public DateTimeOffset? Modified { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public List<AgentConfigSetFuncSettings> Functions { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public List<AgentConfigSetTask> Tasks { get; set; }
    }

    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public class AgentConfigSetTask: AgentConfigTask
    {
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int Id { get; set; }
    }

    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public class AgentConfigSetFuncSettings
    {

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public ModuleFunctionType Type;

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public List<AgentConfigSetFunctionSettings> FunctionSettings { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public Module Module { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public TimeSpan Interval { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public bool Enabled { get; set; } = true;
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public bool Startup { get; set; } = false;


        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public List<string> Targets { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public float WarningThreshold { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public float CriticalThreshold { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public AgentConfigSetCheckAction WarningAction { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public AgentConfigSetCheckAction CriticalAction { get; set; }
    }

    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public class AgentConfigSetFunctionSettings
    {
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Key { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Target { get; set; }
    }

    /// <summary>
    /// TODO: Add text here
    /// </summary>
    [Serializable]
    public class AgentConfigSetCheckAction
    {
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Action { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Module { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Target { get; set; }
    }
}
