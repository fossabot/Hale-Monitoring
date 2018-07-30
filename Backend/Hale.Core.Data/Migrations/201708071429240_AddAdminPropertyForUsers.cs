namespace Hale.Core.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddAdminPropertyForUsers : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("Entities.Accounts", "IsAdmin", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            this.DropColumn("Entities.Accounts", "IsAdmin");
        }
    }
}
