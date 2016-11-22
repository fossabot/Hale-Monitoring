namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public partial class UniqueUsername : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            AlterColumn("dbo.Users", "UserName", c => c.String(maxLength: 450));
            CreateIndex("dbo.Users", "UserName", unique: true);
        }

        /// <inheritdoc />
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "UserName" });
            AlterColumn("dbo.Users", "UserName", c => c.String());
        }
    }
}
