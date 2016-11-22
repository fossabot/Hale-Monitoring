namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public partial class UserFix2 : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            AddColumn("dbo.Users", "FullName", c => c.String());
            AddColumn("dbo.Users", "OldPassword", c => c.String());
            AddColumn("dbo.Users", "PasswordChanged", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "Activated", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "Enabled", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "Email");
        }

        /// <inheritdoc />
        public override void Down()
        {
            AddColumn("dbo.Users", "Email", c => c.String());
            DropColumn("dbo.Users", "Enabled");
            DropColumn("dbo.Users", "Activated");
            DropColumn("dbo.Users", "PasswordChanged");
            DropColumn("dbo.Users", "OldPassword");
            DropColumn("dbo.Users", "FullName");
        }
    }
}
