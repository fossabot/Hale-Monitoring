namespace Hale.Core.Migrations
{
    using Models.Nodes;
    using Models.Users;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Hale.Core.Contexts.HaleDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Hale.Core.Contexts.HaleDBModel";
        }

        protected override void Seed(Hale.Core.Contexts.HaleDBContext context)
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
            var now = DateTime.Now;

            context.Accounts.AddOrUpdate(
                u => u.UserName,
                new Account { UserName = "test01", FullName = "Test User 01", Password = BCrypt.Net.BCrypt.HashPassword("test01", 5) },
                new Account { UserName = "test02", FullName = "Test User 02", Password = BCrypt.Net.BCrypt.HashPassword("test02", 5) }
            );

            context.Hosts.AddOrUpdate(
                h => h.Guid,
                new Host
                {
                    Ip = "127.0.0.1",
                    FriendlyName = "TestHost01",
                    Created = now,
                    Modified = now,
                    Status = (int)Status.Warning,
                    Guid = new Guid("{057449E7-E7F1-47B6-80A7-B21ED8DEA058}"),
                    HostName = "test-host-01.domain.com"
                },
                new Host
                {
                    Ip = "10.1.2.3",
                    FriendlyName = "TestHost02",
                    Created = now,
                    Modified = now,
                    Status = (int)Status.Ok,
                    Guid = new Guid("{FAF16DE0-B8E4-4A0F-8C1B-EB410725C6DA}"),
                    HostName = "test-host-01.domain.com"
                },
                new Host
                {
                    Ip = "10.1.2.4",
                    FriendlyName = "TestHost03",
                    Created = now,
                    Modified = now,
                    Status = (int)Status.Ok,
                    Guid = new Guid("{844FA1AB-3B54-45BF-AB5C-9BEDFCD6AFA8}"),
                    HostName = "test-host-01.domain.com"
                }
            );
        }
    }
}
