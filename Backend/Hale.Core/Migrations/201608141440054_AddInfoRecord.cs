namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInfoRecord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Modules.InfoRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        Result_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Modules.Results", t => t.Result_Id)
                .Index(t => t.Result_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Modules.InfoRecords", "Result_Id", "Modules.Results");
            DropIndex("Modules.InfoRecords", new[] { "Result_Id" });
            DropTable("Modules.InfoRecords");
        }
    }
}
