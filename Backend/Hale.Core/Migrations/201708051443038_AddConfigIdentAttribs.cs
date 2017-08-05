namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConfigIdentAttribs : DbMigration
    {
        public override void Up()
        {
            // NOTE: Written by hand since entity CLI was uncooperative. -NM 2017-08-05

            AddColumn("Agent.AgentConfigSets", "Identifier", c => c.String(nullable: false, maxLength: 32));
            CreateIndex("Agent.AgentConfigSets", "Identifier", unique: true, name: "IX_Identifier_Unique");

            AddColumn("Agent.AgentConfigSets", "Name", c => c.String(defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("Agent.AgentConfigSets", "Name");

            DropIndex("Agent.AgentConfigSets", "IX_Identifier_Unique");
            DropColumn("Agent.AgentConfigSets", "Identifier");

        }
    }
}
