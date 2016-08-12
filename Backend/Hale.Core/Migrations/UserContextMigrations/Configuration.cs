namespace Hale.Core.Migrations.UserContextMigrations
{
    using Models.User;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Hale.Core.Contexts.UserContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\UserContextMigrations";
            ContextKey = "Hale.Core.Contexts.HaleDBModel";

        }

        protected override void Seed(Hale.Core.Contexts.UserContext context)
        {
            context.Accounts.AddOrUpdate(
                 u => u.UserName,
                 new Account { UserName = "test01", FullName = "Test User 01", Password = BCrypt.Net.BCrypt.HashPassword("test01", 5) },
                 new Account { UserName = "test02", FullName = "Test User 02", Password = BCrypt.Net.BCrypt.HashPassword("test02", 5) }
             );
        }
    }
}
