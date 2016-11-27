namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgentHostIdentification : DbMigration
    {
        public override void Up()
        {
            AddColumn("Nodes.Hosts", "Domain", c => c.String());
            AddColumn("Nodes.Hosts", "OperatingSystem", c => c.String());
            AddColumn("Nodes.Hosts", "NicSummary", c => c.String());
            AddColumn("Nodes.Hosts", "HardwareSummary", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Nodes.Hosts", "HardwareSummary");
            DropColumn("Nodes.Hosts", "NicSummary");
            DropColumn("Nodes.Hosts", "OperatingSystem");
            DropColumn("Nodes.Hosts", "Domain");
        }
    }
}
