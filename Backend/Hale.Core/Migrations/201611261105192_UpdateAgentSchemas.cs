namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAgentSchemas : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "Modules.AgentConfigSets", newSchema: "Agent");
            MoveTable(name: "Modules.AgentConfigSetFuncSettings", newSchema: "Agent");
            MoveTable(name: "Modules.AgentConfigSetCheckActions", newSchema: "Agent");
            MoveTable(name: "Modules.AgentConfigSetFunctionSettings", newSchema: "Agent");
            MoveTable(name: "Modules.AgentConfigSetTasks", newSchema: "Agent");
        }
        
        public override void Down()
        {
            MoveTable(name: "Agent.AgentConfigSetTasks", newSchema: "Modules");
            MoveTable(name: "Agent.AgentConfigSetFunctionSettings", newSchema: "Modules");
            MoveTable(name: "Agent.AgentConfigSetCheckActions", newSchema: "Modules");
            MoveTable(name: "Agent.AgentConfigSetFuncSettings", newSchema: "Modules");
            MoveTable(name: "Agent.AgentConfigSets", newSchema: "Modules");
        }
    }
}
