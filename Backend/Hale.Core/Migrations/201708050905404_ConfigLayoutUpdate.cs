namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfigLayoutUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("Agent.AgentConfigSetTasks", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Agent.AgentConfigSetTasks", "Name");
        }
    }
}
