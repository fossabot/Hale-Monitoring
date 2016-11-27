namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgentConfiguredStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("Nodes.Hosts", "Configured", c => c.Boolean(nullable: false));
            AddColumn("Nodes.Hosts", "Blocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Nodes.Hosts", "Blocked");
            DropColumn("Nodes.Hosts", "Configured");
        }
    }
}
