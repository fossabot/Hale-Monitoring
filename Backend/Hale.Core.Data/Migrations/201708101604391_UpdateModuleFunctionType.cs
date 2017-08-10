namespace Hale.Core.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModuleFunctionType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Entities.Functions", "Type", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Entities.Functions", "Type", c => c.Int(nullable: false));
        }
    }
}
