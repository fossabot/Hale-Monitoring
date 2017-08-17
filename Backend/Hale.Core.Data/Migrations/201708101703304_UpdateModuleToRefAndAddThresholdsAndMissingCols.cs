namespace Hale.Core.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateModuleToRefAndAddThresholdsAndMissingCols : DbMigration
    {
        public override void Up()
        {
            this.RenameTable(name: "Entities.AgentConfigSetFuncSettings", newName: "AgentConfigSetFunctions");
            this.RenameColumn(table: "Entities.AgentConfigSetFunctionSettings", name: "AgentConfigSetFuncSettings_Id", newName: "AgentConfigSetFunctions_Id");
            this.RenameIndex(table: "Entities.AgentConfigSetFunctionSettings", name: "IX_AgentConfigSetFuncSettings_Id", newName: "IX_AgentConfigSetFunctions_Id");
            this.AddColumn("Entities.Results", "AboveWarning", c => c.Boolean(nullable: false));
            this.AddColumn("Entities.Results", "AboveCritical", c => c.Boolean(nullable: false));
            this.AddColumn("Entities.Functions", "Identifier", c => c.String());
            this.AddColumn("Entities.Functions", "Description", c => c.String());
            this.AddColumn("Entities.Functions", "Module_Id", c => c.Int());
            this.CreateIndex("Entities.Functions", "Module_Id");
            this.AddForeignKey("Entities.Functions", "Module_Id", "Entities.Modules", "Id");
            this.DropColumn("Entities.Functions", "ModuleId");
        }

        public override void Down()
        {
            this.AddColumn("Entities.Functions", "ModuleId", c => c.Int(nullable: false));
            this.DropForeignKey("Entities.Functions", "Module_Id", "Entities.Modules");
            this.DropIndex("Entities.Functions", new[] { "Module_Id" });
            this.DropColumn("Entities.Functions", "Module_Id");
            this.DropColumn("Entities.Functions", "Description");
            this.DropColumn("Entities.Functions", "Identifier");
            this.DropColumn("Entities.Results", "AboveCritical");
            this.DropColumn("Entities.Results", "AboveWarning");
            this.RenameIndex(table: "Entities.AgentConfigSetFunctionSettings", name: "IX_AgentConfigSetFunctions_Id", newName: "IX_AgentConfigSetFuncSettings_Id");
            this.RenameColumn(table: "Entities.AgentConfigSetFunctionSettings", name: "AgentConfigSetFunctions_Id", newName: "AgentConfigSetFuncSettings_Id");
            this.RenameTable(name: "Entities.AgentConfigSetFunctions", newName: "AgentConfigSetFuncSettings");
        }
    }
}
