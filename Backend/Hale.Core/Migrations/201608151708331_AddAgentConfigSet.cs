namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public partial class AddAgentConfigSet : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            CreateTable(
                "Modules.AgentConfigSets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        Modified = c.DateTime(),
                        ModifiedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Modules.AgentConfigSetFuncSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Function = c.String(),
                        Interval = c.Time(nullable: false, precision: 7),
                        Enabled = c.Boolean(nullable: false),
                        Startup = c.Boolean(nullable: false),
                        WarningThreshold = c.Single(nullable: false),
                        CriticalThreshold = c.Single(nullable: false),
                        CriticalAction_Id = c.Int(),
                        Module_Id = c.Int(),
                        WarningAction_Id = c.Int(),
                        AgentConfigSet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Modules.AgentConfigSetCheckActions", t => t.CriticalAction_Id)
                .ForeignKey("Modules.Modules", t => t.Module_Id)
                .ForeignKey("Modules.AgentConfigSetCheckActions", t => t.WarningAction_Id)
                .ForeignKey("Modules.AgentConfigSets", t => t.AgentConfigSet_Id)
                .Index(t => t.CriticalAction_Id)
                .Index(t => t.Module_Id)
                .Index(t => t.WarningAction_Id)
                .Index(t => t.AgentConfigSet_Id);
            
            CreateTable(
                "Modules.AgentConfigSetCheckActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Action = c.String(),
                        Module = c.String(),
                        Target = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Modules.AgentConfigSetFunctionSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        Target = c.String(),
                        AgentConfigSetFuncSettings_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Modules.AgentConfigSetFuncSettings", t => t.AgentConfigSetFuncSettings_Id)
                .Index(t => t.AgentConfigSetFuncSettings_Id);
            
            CreateTable(
                "Modules.AgentConfigSetTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        Interval = c.Time(nullable: false, precision: 7),
                        Startup = c.Boolean(nullable: false),
                        AgentConfigSet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Modules.AgentConfigSets", t => t.AgentConfigSet_Id)
                .Index(t => t.AgentConfigSet_Id);
            
        }

        /// <inheritdoc />
        public override void Down()
        {
            DropForeignKey("Modules.AgentConfigSetTasks", "AgentConfigSet_Id", "Modules.AgentConfigSets");
            DropForeignKey("Modules.AgentConfigSetFuncSettings", "AgentConfigSet_Id", "Modules.AgentConfigSets");
            DropForeignKey("Modules.AgentConfigSetFuncSettings", "WarningAction_Id", "Modules.AgentConfigSetCheckActions");
            DropForeignKey("Modules.AgentConfigSetFuncSettings", "Module_Id", "Modules.Modules");
            DropForeignKey("Modules.AgentConfigSetFunctionSettings", "AgentConfigSetFuncSettings_Id", "Modules.AgentConfigSetFuncSettings");
            DropForeignKey("Modules.AgentConfigSetFuncSettings", "CriticalAction_Id", "Modules.AgentConfigSetCheckActions");
            DropIndex("Modules.AgentConfigSetTasks", new[] { "AgentConfigSet_Id" });
            DropIndex("Modules.AgentConfigSetFunctionSettings", new[] { "AgentConfigSetFuncSettings_Id" });
            DropIndex("Modules.AgentConfigSetFuncSettings", new[] { "AgentConfigSet_Id" });
            DropIndex("Modules.AgentConfigSetFuncSettings", new[] { "WarningAction_Id" });
            DropIndex("Modules.AgentConfigSetFuncSettings", new[] { "Module_Id" });
            DropIndex("Modules.AgentConfigSetFuncSettings", new[] { "CriticalAction_Id" });
            DropTable("Modules.AgentConfigSetTasks");
            DropTable("Modules.AgentConfigSetFunctionSettings");
            DropTable("Modules.AgentConfigSetCheckActions");
            DropTable("Modules.AgentConfigSetFuncSettings");
            DropTable("Modules.AgentConfigSets");
        }
    }
}
