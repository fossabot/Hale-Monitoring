namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingFunctionTypeEnum : DbMigration
    {
        public override void Up()
        {
            AddColumn("Agent.AgentConfigSetFuncSettings", "Type", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Agent.AgentConfigSetFuncSettings", "Type");
        }
    }
}
