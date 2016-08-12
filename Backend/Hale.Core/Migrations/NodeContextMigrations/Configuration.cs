namespace Hale.Core.Migrations.NodeContextMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Hale.Core.Contexts.NodeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\NodeContextMigrations";
            ContextKey = "Hale.Core.Contexts.HaleDBModel";
        }

        protected override void Seed(Hale.Core.Contexts.NodeContext context)
        {
        }
    }
}
