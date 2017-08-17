namespace Hale.Core.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateModuleFunctionType : DbMigration
    {
        public override void Up()
        {
            this.AlterColumn("Entities.Functions", "Type", c => c.Byte(nullable: false));
        }

        public override void Down()
        {
            this.AlterColumn("Entities.Functions", "Type", c => c.Int(nullable: false));
        }
    }
}
