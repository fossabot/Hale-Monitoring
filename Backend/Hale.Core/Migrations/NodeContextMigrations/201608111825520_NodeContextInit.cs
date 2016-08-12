namespace Hale.Core.Migrations.NodeContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NodeContextInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Node.HostDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HostId = c.Int(nullable: false),
                        Key = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Node.Hosts", t => t.HostId, cascadeDelete: true)
                .Index(t => t.HostId);
            
            CreateTable(
                "Node.Hosts",
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("Node.HostDetails", "HostId", "Node.Hosts");
            DropIndex("Node.HostDetails", new[] { "HostId" });
            DropTable("Node.Hosts");
            DropTable("Node.HostDetails");
        }
    }
}
