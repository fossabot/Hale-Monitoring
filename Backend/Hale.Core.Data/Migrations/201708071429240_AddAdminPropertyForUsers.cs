namespace Hale.Core.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdminPropertyForUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("Entities.Accounts", "IsAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Entities.Accounts", "IsAdmin");
        }
    }
}
