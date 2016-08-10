namespace Hale.Core.Migrations
{
    using Entities.Security;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Hale.Core.Contexts.HaleDBModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Hale.Core.Contexts.HaleDBModel";
        }

        protected override void Seed(Hale.Core.Contexts.HaleDBModel context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Users.AddOrUpdate(
                u => u.UserName,
                new User { UserName = "test01", FullName = "Test User 01", Password = BCrypt.Net.BCrypt.HashPassword("test01", 5) },
                new User { UserName = "test02", FullName = "Test User 02", Password = BCrypt.Net.BCrypt.HashPassword("test02", 5) }
            );
        }
    }
}
