namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFix3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "PasswordChanged", c => c.DateTime());
            AlterColumn("dbo.Users", "CreatedBy", c => c.Int());
            AlterColumn("dbo.Users", "Modified", c => c.DateTime());
            AlterColumn("dbo.Users", "ModifiedBy", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "ModifiedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Modified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "PasswordChanged", c => c.DateTime(nullable: false));
        }
    }
}
