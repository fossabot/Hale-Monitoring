namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SwitchToUTCDates : DbMigration
    {
        public override void Up()
        {
            DropColumn("Users.Accounts", "PasswordChanged"); // Have to do this for MS SQL Server
            AddColumn("Users.Accounts", "PasswordChanged", c => c.DateTimeOffset(precision: 7));

            AlterColumn("Users.Accounts", "Created", c => c.DateTimeOffset(nullable: false, precision: 7));

            DropColumn("Users.Accounts", "Modified");
            AddColumn("Users.Accounts", "Modified", c => c.DateTimeOffset(precision: 7));

            AlterColumn("Agent.AgentConfigSets", "Created", c => c.DateTimeOffset(nullable: false, precision: 7));

            DropColumn("Agent.AgentConfigSets", "Modified");
            AddColumn("Agent.AgentConfigSets", "Modified", c => c.DateTimeOffset(precision: 7));

            AlterColumn("Modules.Results", "ExecutionTime", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("Nodes.HostComments", "Timestamp", c => c.DateTimeOffset(nullable: false, precision: 7));

            DropColumn("Nodes.Hosts", "LastConnected");
            AddColumn("Nodes.Hosts", "LastConnected", c => c.DateTimeOffset(precision: 7));

            DropColumn("Nodes.Hosts", "Modified");
            AddColumn("Nodes.Hosts", "Modified", c => c.DateTimeOffset(precision: 7));

            AlterColumn("Nodes.Hosts", "Created", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("Nodes.Hosts", "Created", c => c.DateTime(nullable: false));
            AlterColumn("Nodes.Hosts", "Modified", c => c.DateTime(nullable: false));
            AlterColumn("Nodes.Hosts", "LastConnected", c => c.DateTime());
            AlterColumn("Nodes.HostComments", "Timestamp", c => c.DateTime(nullable: false));
            AlterColumn("Modules.Results", "ExecutionTime", c => c.DateTime(nullable: false));
            AlterColumn("Agent.AgentConfigSets", "Modified", c => c.DateTime());
            AlterColumn("Agent.AgentConfigSets", "Created", c => c.DateTime(nullable: false));
            AlterColumn("Users.Accounts", "Modified", c => c.DateTime());
            AlterColumn("Users.Accounts", "Created", c => c.DateTime(nullable: false));
            AlterColumn("Users.Accounts", "PasswordChanged", c => c.DateTime());
        }

    }
}
