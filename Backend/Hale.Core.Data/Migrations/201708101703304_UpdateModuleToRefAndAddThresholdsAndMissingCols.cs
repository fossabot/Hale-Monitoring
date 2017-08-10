namespace Hale.Core.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModuleToRefAndAddThresholdsAndMissingCols : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Entities.AgentConfigSetFuncSettings", newName: "AgentConfigSetFunctions");
            RenameColumn(table: "Entities.AgentConfigSetFunctionSettings", name: "AgentConfigSetFuncSettings_Id", newName: "AgentConfigSetFunctions_Id");
            RenameIndex(table: "Entities.AgentConfigSetFunctionSettings", name: "IX_AgentConfigSetFuncSettings_Id", newName: "IX_AgentConfigSetFunctions_Id");
            AddColumn("Entities.Results", "AboveWarning", c => c.Boolean(nullable: false));
            AddColumn("Entities.Results", "AboveCritical", c => c.Boolean(nullable: false));
            AddColumn("Entities.Functions", "Identifier", c => c.String());
            AddColumn("Entities.Functions", "Description", c => c.String());
            AddColumn("Entities.Functions", "Module_Id", c => c.Int());
            CreateIndex("Entities.Functions", "Module_Id");
            AddForeignKey("Entities.Functions", "Module_Id", "Entities.Modules", "Id");
            DropColumn("Entities.Functions", "ModuleId");
        }
        
        public override void Down()
        {
            AddColumn("Entities.Functions", "ModuleId", c => c.Int(nullable: false));
            DropForeignKey("Entities.Functions", "Module_Id", "Entities.Modules");
            DropIndex("Entities.Functions", new[] { "Module_Id" });
            DropColumn("Entities.Functions", "Module_Id");
            DropColumn("Entities.Functions", "Description");
            DropColumn("Entities.Functions", "Identifier");
            DropColumn("Entities.Results", "AboveCritical");
            DropColumn("Entities.Results", "AboveWarning");
            RenameIndex(table: "Entities.AgentConfigSetFunctionSettings", name: "IX_AgentConfigSetFunctions_Id", newName: "IX_AgentConfigSetFuncSettings_Id");
            RenameColumn(table: "Entities.AgentConfigSetFunctionSettings", name: "AgentConfigSetFunctions_Id", newName: "AgentConfigSetFuncSettings_Id");
            RenameTable(name: "Entities.AgentConfigSetFunctions", newName: "AgentConfigSetFuncSettings");
        }
    }
}
