namespace Hale.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public partial class HostNullableLastConnected : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            AlterColumn("Nodes.Hosts", "LastConnected", c => c.DateTime());
        }

        /// <inheritdoc />
        public override void Down()
        {
            AlterColumn("Nodes.Hosts", "LastConnected", c => c.DateTime(nullable: false));
        }
    }
}
