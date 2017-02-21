namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentsAndEmail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Nodes.HostComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Text = c.String(),
                        Node_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Nodes.Hosts", t => t.Node_Id)
                .ForeignKey("Users.Accounts", t => t.User_Id)
                .Index(t => t.Node_Id)
                .Index(t => t.User_Id);
            
            AddColumn("Users.Accounts", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("Nodes.HostComments", "User_Id", "Users.Accounts");
            DropForeignKey("Nodes.HostComments", "Node_Id", "Nodes.Hosts");
            DropIndex("Nodes.HostComments", new[] { "User_Id" });
            DropIndex("Nodes.HostComments", new[] { "Node_Id" });
            DropColumn("Users.Accounts", "Email");
            DropTable("Nodes.HostComments");
        }
    }
}
