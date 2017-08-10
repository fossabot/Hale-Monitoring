namespace Hale.Core.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RevertSchemaNames : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "Entities.AccountDetails", newSchema: "Users");
            MoveTable(name: "Entities.Accounts", newSchema: "Users");
            MoveTable(name: "Entities.AgentConfigSets", newSchema: "Agent");
            MoveTable(name: "Entities.AgentConfigSetFunctions", newSchema: "Agent");
            MoveTable(name: "Entities.AgentConfigSetCheckActions", newSchema: "Agent");
            MoveTable(name: "Entities.AgentConfigSetFunctionSettings", newSchema: "Agent");
            MoveTable(name: "Entities.Modules", newSchema: "Modules");
            MoveTable(name: "Entities.AgentConfigSetTasks", newSchema: "Agent");
            MoveTable(name: "Entities.CheckRecords", newSchema: "Modules");
            MoveTable(name: "Entities.Results", newSchema: "Modules");
            MoveTable(name: "Entities.Functions", newSchema: "Modules");
            MoveTable(name: "Entities.InfoRecords", newSchema: "Modules");
            MoveTable(name: "Entities.NodeComments", newSchema: "Nodes");
            MoveTable(name: "Entities.Nodes", newSchema: "Nodes");
            MoveTable(name: "Entities.NodeDetails", newSchema: "Nodes");
        }
        
        public override void Down()
        {
            MoveTable(name: "Nodes.NodeDetails", newSchema: "Entities");
            MoveTable(name: "Nodes.Nodes", newSchema: "Entities");
            MoveTable(name: "Nodes.NodeComments", newSchema: "Entities");
            MoveTable(name: "Modules.InfoRecords", newSchema: "Entities");
            MoveTable(name: "Modules.Functions", newSchema: "Entities");
            MoveTable(name: "Modules.Results", newSchema: "Entities");
            MoveTable(name: "Modules.CheckRecords", newSchema: "Entities");
            MoveTable(name: "Agent.AgentConfigSetTasks", newSchema: "Entities");
            MoveTable(name: "Modules.Modules", newSchema: "Entities");
            MoveTable(name: "Agent.AgentConfigSetFunctionSettings", newSchema: "Entities");
            MoveTable(name: "Agent.AgentConfigSetCheckActions", newSchema: "Entities");
            MoveTable(name: "Agent.AgentConfigSetFunctions", newSchema: "Entities");
            MoveTable(name: "Agent.AgentConfigSets", newSchema: "Entities");
            MoveTable(name: "Users.Accounts", newSchema: "Entities");
            MoveTable(name: "Users.AccountDetails", newSchema: "Entities");
        }
    }
}
