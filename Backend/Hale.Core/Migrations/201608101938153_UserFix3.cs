namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public partial class UserFix3 : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            AlterColumn("dbo.Users", "PasswordChanged", c => c.DateTime());
            AlterColumn("dbo.Users", "CreatedBy", c => c.Int());
            AlterColumn("dbo.Users", "Modified", c => c.DateTime());
            AlterColumn("dbo.Users", "ModifiedBy", c => c.Int());
        }

        /// <inheritdoc />
        public override void Down()
        {
            AlterColumn("dbo.Users", "ModifiedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Modified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "PasswordChanged", c => c.DateTime(nullable: false));
        }
    }
}
