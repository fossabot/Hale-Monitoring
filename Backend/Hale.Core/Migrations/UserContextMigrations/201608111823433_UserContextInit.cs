namespace Hale.Core.Migrations.UserContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserContextInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "User.AccountDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Key = c.String(),
                        Value = c.String(),
                        Account_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("User.Accounts", t => t.Account_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "User.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 450),
                        FullName = c.String(),
                        Password = c.String(),
                        OldPassword = c.String(),
                        PasswordChanged = c.DateTime(),
                        Activated = c.Boolean(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        Modified = c.DateTime(),
                        ModifiedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("User.AccountDetails", "Account_Id", "User.Accounts");
            DropIndex("User.Accounts", new[] { "UserName" });
            DropIndex("User.AccountDetails", new[] { "Account_Id" });
            DropTable("User.Accounts");
            DropTable("User.AccountDetails");
        }
    }
}
