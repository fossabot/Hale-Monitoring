namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    /// Migration that adds modified- and configured by to the hosts table
    /// </summary>
    public partial class AddModifiedAndConfiguredByToHost : DbMigration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            AddColumn("Nodes.Hosts", "ModifiedBy", c => c.Int());
            AddColumn("Nodes.Hosts", "ConfiguredBy", c => c.Int());
        }

        /// <inheritdoc/>
        public override void Down()
        {
            DropColumn("Nodes.Hosts", "ConfiguredBy");
            DropColumn("Nodes.Hosts", "ModifiedBy");
        }
    }
}
