namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HostNullableLastConnected : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Nodes.Hosts", "LastConnected", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("Nodes.Hosts", "LastConnected", c => c.DateTime(nullable: false));
        }
    }
}
