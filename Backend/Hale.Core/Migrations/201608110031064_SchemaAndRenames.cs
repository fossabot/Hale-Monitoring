namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public partial class SchemaAndRenames : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            RenameTable(name: "dbo.UserDetails", newName: "AccountDetails");
            RenameTable(name: "dbo.Users", newName: "Accounts");
            MoveTable(name: "dbo.AccountDetails", newSchema: "User");
            MoveTable(name: "dbo.Accounts", newSchema: "User");

            //DropIndex("User.AccountDetails", new[] { "UserId" });
            AddColumn("User.AccountDetails", "Account_Id", c => c.Int());
            CreateIndex("User.AccountDetails", "Account_Id");
            AddForeignKey("User.AccountDetails", "Account_Id", "User.Accounts", "Id");
        }

        /// <inheritdoc />
        public override void Down()
        {
            DropForeignKey("User.AccountDetails", "Account_Id", "User.Accounts");
            DropIndex("User.AccountDetails", new[] { "Account_Id" });
            DropColumn("User.AccountDetails", "Account_Id");
            CreateIndex("User.AccountDetails", "UserId");

            MoveTable(name: "User.Accounts", newSchema: "dbo");
            MoveTable(name: "User.AccountDetails", newSchema: "dbo");
            RenameTable(name: "dbo.Accounts", newName: "Users");
            RenameTable(name: "dbo.AccountDetails", newName: "UserDetails");
        }
    }
}
