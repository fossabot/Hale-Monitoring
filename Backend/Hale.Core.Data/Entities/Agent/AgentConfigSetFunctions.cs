using System;
using System.Collections.Generic;
using Hale.Lib.Modules;
using EModule = Hale.Core.Data.Entities.Modules.Module;

#pragma warning disable 108, 114

namespace Hale.Core.Data.Entities.Agent
{
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public class AgentConfigSetFunctions
    {

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public ModuleFunctionType Type { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public List<AgentConfigSetFunctionSettings> FunctionSettings { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public EModule Module { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Function { get; set; } // TODO: Make this a reference to Function -NM 2017-08-10

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
}
