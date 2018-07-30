namespace Hale.Core.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Init : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "Entities.AccountDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Key = c.String(),
                        Value = c.String(),
                        Account_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Entities.Accounts", t => t.Account_Id)
                .Index(t => t.Account_Id);

            this.CreateTable(
                "Entities.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 450),
                        Email = c.String(),
                        FullName = c.String(),
                        Password = c.String(),
                        OldPassword = c.String(),
                        PasswordChanged = c.DateTimeOffset(precision: 7),
                        Activated = c.Boolean(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedBy = c.Int(),
                        Modified = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true);

            this.CreateTable(
                "Entities.AgentConfigSets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identifier = c.String(maxLength: 32),
                        Name = c.String(),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedBy = c.Int(),
                        Modified = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Identifier, unique: true, name: "IX_Identifier_Unique");

            this.CreateTable(
                "Entities.AgentConfigSetFuncSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Byte(nullable: false),
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
                .ForeignKey("Entities.AgentConfigSetCheckActions", t => t.CriticalAction_Id)
                .ForeignKey("Entities.Modules", t => t.Module_Id)
                .ForeignKey("Entities.AgentConfigSetCheckActions", t => t.WarningAction_Id)
                .ForeignKey("Entities.AgentConfigSets", t => t.AgentConfigSet_Id)
                .Index(t => t.CriticalAction_Id)
                .Index(t => t.Module_Id)
                .Index(t => t.WarningAction_Id)
                .Index(t => t.AgentConfigSet_Id);

            this.CreateTable(
                "Entities.AgentConfigSetCheckActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Action = c.String(),
                        Module = c.String(),
                        Target = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "Entities.AgentConfigSetFunctionSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        Target = c.String(),
                        AgentConfigSetFuncSettings_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Entities.AgentConfigSetFuncSettings", t => t.AgentConfigSetFuncSettings_Id)
                .Index(t => t.AgentConfigSetFuncSettings_Id);

            this.CreateTable(
                "Entities.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identifier = c.String(),
                        Major = c.Int(nullable: false),
                        Minor = c.Int(nullable: false),
                        Revision = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "Entities.AgentConfigSetTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Enabled = c.Boolean(nullable: false),
                        Interval = c.Time(nullable: false, precision: 7),
                        Startup = c.Boolean(nullable: false),
                        AgentConfigSet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Entities.AgentConfigSets", t => t.AgentConfigSet_Id)
                .Index(t => t.AgentConfigSet_Id);

            this.CreateTable(
                "Entities.CheckRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.Double(nullable: false),
                        Result_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Entities.Results", t => t.Result_Id)
                .Index(t => t.Result_Id);

            this.CreateTable(
                "Entities.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HostId = c.Int(nullable: false),
                        ModuleId = c.Int(nullable: false),
                        FunctionId = c.Int(nullable: false),
                        Target = c.String(),
                        ExecutionTime = c.DateTimeOffset(nullable: false, precision: 7),
                        Message = c.String(),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "Entities.Functions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        ModuleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "Entities.InfoRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        Result_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Entities.Results", t => t.Result_Id)
                .Index(t => t.Result_Id);

            this.CreateTable(
                "Entities.NodeComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTimeOffset(nullable: false, precision: 7),
                        Text = c.String(),
                        Node_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Entities.Nodes", t => t.Node_Id)
                .ForeignKey("Entities.Accounts", t => t.User_Id)
                .Index(t => t.Node_Id)
                .Index(t => t.User_Id);

            this.CreateTable(
                "Entities.Nodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FriendlyName = c.String(),
                        HostName = c.String(),
                        Domain = c.String(),
                        OperatingSystem = c.String(),
                        NicSummary = c.String(),
                        HardwareSummary = c.String(),
                        Ip = c.String(),
                        Status = c.Int(nullable: false),
                        LastConnected = c.DateTimeOffset(precision: 7),
                        Modified = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.Int(),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                        ConfiguredBy = c.Int(),
                        Guid = c.Guid(nullable: false),
                        RsaKey = c.Binary(),
                        Configured = c.Boolean(nullable: false),
                        Blocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "Entities.NodeDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HostId = c.Int(nullable: false),
                        Key = c.String(),
                        Value = c.String(),
                        Node_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Entities.Nodes", t => t.Node_Id)
                .Index(t => t.Node_Id);
        }

        public override void Down()
        {
            this.DropForeignKey("Entities.NodeComments", "User_Id", "Entities.Accounts");
            this.DropForeignKey("Entities.NodeComments", "Node_Id", "Entities.Nodes");
            this.DropForeignKey("Entities.NodeDetails", "Node_Id", "Entities.Nodes");
            this.DropForeignKey("Entities.InfoRecords", "Result_Id", "Entities.Results");
            this.DropForeignKey("Entities.CheckRecords", "Result_Id", "Entities.Results");
            this.DropForeignKey("Entities.AgentConfigSetTasks", "AgentConfigSet_Id", "Entities.AgentConfigSets");
            this.DropForeignKey("Entities.AgentConfigSetFuncSettings", "AgentConfigSet_Id", "Entities.AgentConfigSets");
            this.DropForeignKey("Entities.AgentConfigSetFuncSettings", "WarningAction_Id", "Entities.AgentConfigSetCheckActions");
            this.DropForeignKey("Entities.AgentConfigSetFuncSettings", "Module_Id", "Entities.Modules");
            this.DropForeignKey("Entities.AgentConfigSetFunctionSettings", "AgentConfigSetFuncSettings_Id", "Entities.AgentConfigSetFuncSettings");
            this.DropForeignKey("Entities.AgentConfigSetFuncSettings", "CriticalAction_Id", "Entities.AgentConfigSetCheckActions");
            this.DropForeignKey("Entities.AccountDetails", "Account_Id", "Entities.Accounts");
            this.DropIndex("Entities.NodeDetails", new[] { "Node_Id" });
            this.DropIndex("Entities.NodeComments", new[] { "User_Id" });
            this.DropIndex("Entities.NodeComments", new[] { "Node_Id" });
            this.DropIndex("Entities.InfoRecords", new[] { "Result_Id" });
            this.DropIndex("Entities.CheckRecords", new[] { "Result_Id" });
            this.DropIndex("Entities.AgentConfigSetTasks", new[] { "AgentConfigSet_Id" });
            this.DropIndex("Entities.AgentConfigSetFunctionSettings", new[] { "AgentConfigSetFuncSettings_Id" });
            this.DropIndex("Entities.AgentConfigSetFuncSettings", new[] { "AgentConfigSet_Id" });
            this.DropIndex("Entities.AgentConfigSetFuncSettings", new[] { "WarningAction_Id" });
            this.DropIndex("Entities.AgentConfigSetFuncSettings", new[] { "Module_Id" });
            this.DropIndex("Entities.AgentConfigSetFuncSettings", new[] { "CriticalAction_Id" });
            this.DropIndex("Entities.AgentConfigSets", "IX_Identifier_Unique");
            this.DropIndex("Entities.Accounts", new[] { "UserName" });
            this.DropIndex("Entities.AccountDetails", new[] { "Account_Id" });
            this.DropTable("Entities.NodeDetails");
            this.DropTable("Entities.Nodes");
            this.DropTable("Entities.NodeComments");
            this.DropTable("Entities.InfoRecords");
            this.DropTable("Entities.Functions");
            this.DropTable("Entities.Results");
            this.DropTable("Entities.CheckRecords");
            this.DropTable("Entities.AgentConfigSetTasks");
            this.DropTable("Entities.Modules");
            this.DropTable("Entities.AgentConfigSetFunctionSettings");
            this.DropTable("Entities.AgentConfigSetCheckActions");
            this.DropTable("Entities.AgentConfigSetFuncSettings");
            this.DropTable("Entities.AgentConfigSets");
            this.DropTable("Entities.Accounts");
            this.DropTable("Entities.AccountDetails");
        }
    }
}
