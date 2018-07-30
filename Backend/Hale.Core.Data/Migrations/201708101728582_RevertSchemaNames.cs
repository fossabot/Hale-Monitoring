namespace Hale.Core.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RevertSchemaNames : DbMigration
    {
        public override void Up()
        {
            this.MoveTable(name: "Entities.AccountDetails", newSchema: "Users");
            this.MoveTable(name: "Entities.Accounts", newSchema: "Users");
            this.MoveTable(name: "Entities.AgentConfigSets", newSchema: "Agent");
            this.MoveTable(name: "Entities.AgentConfigSetFunctions", newSchema: "Agent");
            this.MoveTable(name: "Entities.AgentConfigSetCheckActions", newSchema: "Agent");
            this.MoveTable(name: "Entities.AgentConfigSetFunctionSettings", newSchema: "Agent");
            this.MoveTable(name: "Entities.Modules", newSchema: "Modules");
            this.MoveTable(name: "Entities.AgentConfigSetTasks", newSchema: "Agent");
            this.MoveTable(name: "Entities.CheckRecords", newSchema: "Modules");
            this.MoveTable(name: "Entities.Results", newSchema: "Modules");
            this.MoveTable(name: "Entities.Functions", newSchema: "Modules");
            this.MoveTable(name: "Entities.InfoRecords", newSchema: "Modules");
            this.MoveTable(name: "Entities.NodeComments", newSchema: "Nodes");
            this.MoveTable(name: "Entities.Nodes", newSchema: "Nodes");
            this.MoveTable(name: "Entities.NodeDetails", newSchema: "Nodes");
        }

        public override void Down()
        {
            this.MoveTable(name: "Nodes.NodeDetails", newSchema: "Entities");
            this.MoveTable(name: "Nodes.Nodes", newSchema: "Entities");
            this.MoveTable(name: "Nodes.NodeComments", newSchema: "Entities");
            this.MoveTable(name: "Modules.InfoRecords", newSchema: "Entities");
            this.MoveTable(name: "Modules.Functions", newSchema: "Entities");
            this.MoveTable(name: "Modules.Results", newSchema: "Entities");
            this.MoveTable(name: "Modules.CheckRecords", newSchema: "Entities");
            this.MoveTable(name: "Agent.AgentConfigSetTasks", newSchema: "Entities");
            this.MoveTable(name: "Modules.Modules", newSchema: "Entities");
            this.MoveTable(name: "Agent.AgentConfigSetFunctionSettings", newSchema: "Entities");
            this.MoveTable(name: "Agent.AgentConfigSetCheckActions", newSchema: "Entities");
            this.MoveTable(name: "Agent.AgentConfigSetFunctions", newSchema: "Entities");
            this.MoveTable(name: "Agent.AgentConfigSets", newSchema: "Entities");
            this.MoveTable(name: "Users.Accounts", newSchema: "Entities");
            this.MoveTable(name: "Users.AccountDetails", newSchema: "Entities");
        }
    }
}
