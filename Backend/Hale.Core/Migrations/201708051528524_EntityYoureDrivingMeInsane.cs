namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntityYoureDrivingMeInsane : DbMigration
    {
        public override void Up()
        {
            DropIndex("Agent.AgentConfigSets", "IX_Identifier_Unique");
            AlterColumn("Agent.AgentConfigSets", "Identifier", c => c.String(maxLength: 32));
            CreateIndex("Agent.AgentConfigSets", "Identifier", unique: true, name: "IX_Identifier_Unique");
        }
        
        public override void Down()
        {
            DropIndex("Agent.AgentConfigSets", "IX_Identifier_Unique");
            AlterColumn("Agent.AgentConfigSets", "Identifier", c => c.String());
            CreateIndex("Agent.AgentConfigSets", "Identifier", unique: true, name: "IX_Identifier_Unique");
        }
    }
}
