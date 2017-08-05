namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SyncUpMisc : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Users.Accounts", "PasswordChanged", c => c.DateTime());
            AlterColumn("Users.Accounts", "Created", c => c.DateTime(nullable: false));
            AlterColumn("Users.Accounts", "Modified", c => c.DateTime());
            AlterColumn("Agent.AgentConfigSets", "Created", c => c.DateTime(nullable: false));
            AlterColumn("Agent.AgentConfigSets", "Modified", c => c.DateTime());
            AlterColumn("Agent.AgentConfigSetFuncSettings", "Interval", c => c.Time(nullable: false, precision: 6));
            AlterColumn("Agent.AgentConfigSetTasks", "Interval", c => c.Time(nullable: false, precision: 6));
            AlterColumn("Modules.Results", "ExecutionTime", c => c.DateTime(nullable: false));
            AlterColumn("Nodes.HostComments", "Timestamp", c => c.DateTime(nullable: false));
            AlterColumn("Nodes.Hosts", "LastConnected", c => c.DateTime());
            AlterColumn("Nodes.Hosts", "Modified", c => c.DateTime(nullable: false));
            AlterColumn("Nodes.Hosts", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Nodes.Hosts", "Created", c => c.DateTime(nullable: false));
            AlterColumn("Nodes.Hosts", "Modified", c => c.DateTime(nullable: false));
            AlterColumn("Nodes.Hosts", "LastConnected", c => c.DateTime());
            AlterColumn("Nodes.HostComments", "Timestamp", c => c.DateTime(nullable: false));
            AlterColumn("Modules.Results", "ExecutionTime", c => c.DateTime(nullable: false));
            AlterColumn("Agent.AgentConfigSetTasks", "Interval", c => c.Time(nullable: false, precision: 7));
            AlterColumn("Agent.AgentConfigSetFuncSettings", "Interval", c => c.Time(nullable: false, precision: 7));
            AlterColumn("Agent.AgentConfigSets", "Modified", c => c.DateTime());
            AlterColumn("Agent.AgentConfigSets", "Created", c => c.DateTime(nullable: false));
            AlterColumn("Users.Accounts", "Modified", c => c.DateTime());
            AlterColumn("Users.Accounts", "Created", c => c.DateTime(nullable: false));
            AlterColumn("Users.Accounts", "PasswordChanged", c => c.DateTime());
        }
    }
}
