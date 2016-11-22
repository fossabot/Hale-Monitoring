namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public partial class InitialCreate : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        Changed = c.DateTime(nullable: false),
                        ChangedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Key = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }

        /// <inheritdoc />
        public override void Down()
        {
            DropForeignKey("dbo.UserDetails", "UserId", "dbo.Users");
            DropIndex("dbo.UserDetails", new[] { "UserId" });
            DropTable("dbo.UserDetails");
            DropTable("dbo.Users");
        }
    }
}
