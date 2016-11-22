namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    /// Add text here
    /// </summary>
    public partial class PluralSchemasAddModules : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            MoveTable(name: "User.AccountDetails", newSchema: "Users");
            MoveTable(name: "User.Accounts", newSchema: "Users");
            CreateTable(
                "Modules.Functions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        ModuleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Modules.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identifier = c.String(),
                        Major = c.Int(nullable: false),
                        Minor = c.Int(nullable: false),
                        Revision = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Modules.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HostId = c.Int(nullable: false),
                        ModuleId = c.Int(nullable: false),
                        FunctionId = c.Int(nullable: false),
                        Target = c.String(),
                        ExecutionTime = c.DateTime(nullable: false),
                        Message = c.String(),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Checks.Checks",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Identifier = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Modules.Functions", t => t.Id)
                .Index(t => t.Id);
            
        }

        /// <inheritdoc />
        public override void Down()
        {
            DropForeignKey("Checks.Checks", "Id", "Modules.Functions");
            DropIndex("Checks.Checks", new[] { "Id" });
            DropTable("Checks.Checks");
            DropTable("Modules.Results");
            DropTable("Modules.Modules");
            DropTable("Modules.Functions");
            MoveTable(name: "Users.Accounts", newSchema: "User");
            MoveTable(name: "Users.AccountDetails", newSchema: "User");
        }
    }
}
