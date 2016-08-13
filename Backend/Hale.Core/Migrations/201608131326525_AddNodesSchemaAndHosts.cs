namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNodesSchemaAndHosts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Nodes.Hosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FriendlyName = c.String(),
                        HostName = c.String(),
                        Ip = c.String(),
                        Status = c.Int(nullable: false),
                        LastConnected = c.DateTime(nullable: false),
                        Modified = c.DateTime(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Guid = c.Guid(nullable: false),
                        RsaKey = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Nodes.HostDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HostId = c.Int(nullable: false),
                        Key = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Nodes.Hosts", t => t.HostId, cascadeDelete: true)
                .Index(t => t.HostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Nodes.HostDetails", "HostId", "Nodes.Hosts");
            DropIndex("Nodes.HostDetails", new[] { "HostId" });
            DropTable("Nodes.HostDetails");
            DropTable("Nodes.Hosts");
        }
    }
}
