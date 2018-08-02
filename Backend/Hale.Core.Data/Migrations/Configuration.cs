namespace Hale.Core.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Hale.Core.Data.Contexts;
    using Hale.Core.Data.Entities.Agent;
    using Hale.Core.Data.Entities.Nodes;
    using Hale.Core.Data.Entities.Users;
    using Hale.Lib.Modules;
    using Semver;
    using EModule = Entities.Modules.Module;

    internal sealed class Configuration : DbMigrationsConfiguration<HaleDBContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
            this.ContextKey = "Hale.Core.Contexts.HaleDBModel";
        }

        protected override void Seed(HaleDBContext context)
        {
            var now = DateTimeOffset.Now;

            context.Accounts.AddOrUpdate(
                u => u.UserName,
                new Account
                {
                    UserName = "test01",
                    Email = "simon.aronsson@outlook.com",
                    FullName = "Test User 01",
                    Password = BCrypt.Net.BCrypt.HashPassword("test01", 5),
                    IsAdmin = true,
                    Enabled = true,
                    Activated = true,
                },
                new Account
                {
                    UserName = "test02",
                    Email = "nils@piksel.se",
                    FullName = "Test User 02",
                    Password = BCrypt.Net.BCrypt.HashPassword("test02", 5),
                    Enabled = true,
                    Activated = true,
                },
                new Account
                {
                    UserName = "test03",
                    Email = "foo@bar.com",
                    FullName = "Test User 03",
                    Password = BCrypt.Net.BCrypt.HashPassword("test03", 5),
                    Enabled = true,
                    Activated = false,
                },
                new Account
                {
                    UserName = "test04",
                    Email = "foobar@acmegroup.com",
                    FullName = "Test User 04",
                    Password = BCrypt.Net.BCrypt.HashPassword("test04", 5),
                    Enabled = false,
                    Activated = true,
                });

            context.Nodes.AddOrUpdate(
                h => h.Guid,
                new Node
                {
                    Ip = "127.0.0.1",
                    FriendlyName = "TestHost01",
                    Created = now,
                    Modified = now,
                    Status = Status.Warning,
                    Guid = new Guid("{057449E7-E7F1-47B6-80A7-B21ED8DEA058}"),
                    HostName = "test-host-01",
                    Domain = "domain.com",
                    Configured = true
                },
                new Node
                {
                    Ip = "10.1.2.2",
                    FriendlyName = "TestHost02",
                    Created = now,
                    Modified = now,
                    Status = Status.Ok,
                    Guid = new Guid("{FAF16DE0-B8E4-4A0F-8C1B-EB410725C6DA}"),
                    HostName = "test-host-02",
                    Domain = "domain.com",
                    Configured = true
                },
                new Node
                {
                    Ip = "10.1.2.3",
                    FriendlyName = "TestHost03",
                    Created = now,
                    Modified = now,
                    Status = Status.Ok,
                    Guid = new Guid("{844FA1AB-3B54-45BF-AB5C-9BEDFCD6AFA9}"),
                    HostName = "test-host-03",
                    Domain = "domain.com",
                    Configured = true
                },
                 new Node
                 {
                     Ip = "10.1.2.4",
                     FriendlyName = "TestHost04",
                     Created = now,
                     Modified = now,
                     Status = Status.Warning,
                     Guid = new Guid("{057449E7-E7F1-47B6-80A7-B21ED8DEA051}"),
                     HostName = "test-host-04",
                     Domain = "domain.com",
                     Configured = true
                 },
                new Node
                {
                    Ip = "10.1.2.5",
                    FriendlyName = "TestHost05",
                    Created = now,
                    Modified = now,
                    Status = Status.Ok,
                    Guid = new Guid("{FAF16DE0-B8E4-4A0F-8C1B-EB410725C6D2}"),
                    HostName = "test-host-05",
                    Domain = "domain.com",
                    Configured = true
                },
                new Node
                {
                    Ip = "10.1.2.6",
                    FriendlyName = "TestHost06",
                    Created = now,
                    Modified = now,
                    Status = Status.Ok,
                    Guid = new Guid("{844FA1AB-3B54-45BF-AB5C-9BEDFCD6AFA3}"),
                    HostName = "test-host-06",
                    Domain = "domain.com",
                    Configured = true
                },
                 new Node
                 {
                     Ip = "10.1.2.7",
                     FriendlyName = "TestHost07",
                     Created = now,
                     Modified = now,
                     Status = Status.Warning,
                     Guid = new Guid("{057449E7-E7F1-47B6-80A7-B21ED8DEA044}"),
                     HostName = "test-host-01",
                     Domain = "domain.com",
                     Configured = true
                 },
                new Node
                {
                    Ip = "10.1.2.8",
                    FriendlyName = "TestHost08",
                    Created = now,
                    Modified = now,
                    Status = Status.Ok,
                    Guid = new Guid("{FAF16DE0-B8E4-4A0F-8C1B-EB410725C6D5}"),
                    HostName = "test-host-01",
                    Domain = "domain.com",
                    Configured = true
                },
                new Node
                {
                    Ip = "10.1.2.9",
                    FriendlyName = "TestHost09",
                    Created = now,
                    Modified = now,
                    Status = Status.Ok,
                    Guid = new Guid("{844FA1AB-3B54-45BF-AB5C-9BEDFCD6AFA6}"),
                    HostName = "test-host-01",
                    Domain = "domain.com",
                    Configured = false,
                    OperatingSystem = "Microsoft Windows NT 10.0.14393.0"
                },
                 new Node
                 {
                     Ip = "10.1.2.10",
                     FriendlyName = "TestHost10",
                     Created = now,
                     Modified = now,
                     Status = Status.Warning,
                     Guid = new Guid("{057449E7-E7F1-47B6-80A7-B21ED8DEA05B}"),
                     HostName = "test-host-01",
                     Domain = "domain.com",
                     Configured = false,
                     OperatingSystem = "Microsoft Windows NT 10.0.14393.0"
                 },
                new Node
                {
                    Ip = "10.1.2.11",
                    FriendlyName = "TestHost11",
                    Created = now,
                    Modified = now,
                    Status = Status.Ok,
                    Guid = new Guid("{FAF16DE0-B8E4-4A0F-8C1B-EB410725C6DD}"),
                    HostName = "test-host-01",
                    Domain = "domain.com",
                    Configured = false,
                    OperatingSystem = "Microsoft Windows NT 10.0.14393.0"
                },
                new Node
                {
                    Ip = "10.1.2.12",
                    FriendlyName = "TestHost12",
                    Created = now,
                    Modified = now,
                    Status = Status.Ok,
                    Guid = new Guid("{844FA1AB-3B54-45BF-AB5C-9BEDFCD6AFBB}"),
                    HostName = "test-host-01",
                    Domain = "domain.com",
                    Configured = false,
                    OperatingSystem = "Microsoft Windows NT 10.0.14393.0"
                },
                new Node
                {
                    Ip = "10.1.2.13",
                    FriendlyName = "TestHost13",
                    Created = now,
                    Modified = now,
                    Status = Status.Ok,
                    Guid = new Guid("{844FA1AB-3B54-45BF-AB5C-9BEDFCD6AFCC}"),
                    HostName = "test-host-01",
                    Domain = "domain.com",
                    Configured = false,
                    OperatingSystem = "Microsoft Windows NT 10.0.14393.0"
                });

            var taskUpload = new AgentConfigSetTask()
            {
                Enabled = true,
                Startup = true,
                Interval = TimeSpan.FromMinutes(5),
                Name = "uploadResults"
            };

            var taskPersist = new AgentConfigSetTask()
            {
                Enabled = true,
                Startup = true,
                Interval = TimeSpan.FromMinutes(2),
                Name = "persistResults",
            };

            var taskHeartbeat = new AgentConfigSetTask()
            {
                Enabled = true,
                Startup = true,
                Interval = TimeSpan.FromMinutes(1),
                Name = "sendHeartbeat",
            };

            var agentConfig = AgentConfigSet.Empty;

            agentConfig.Identifier = "seed_config01";

            agentConfig.Functions.Add(new AgentConfigSetFunctions()
            {
                Enabled = true,
                Function = "default",
                Module = new EModule()
                {
                    Identifier = "com.itshale.core.memory",
                    Version = new SemVersion(1, 0, 0),
                },
                Startup = true,
                FunctionSettings = new List<AgentConfigSetFunctionSettings>()
                {
                    new AgentConfigSetFunctionSettings()
                    {
                        Target = "default",
                        Key = "foo",
                        Value = "bar"
                    }
                },
                Interval = TimeSpan.FromMinutes(10),
                Type = ModuleFunctionType.Check
            });

            agentConfig.Tasks.Add(taskUpload);
            agentConfig.Tasks.Add(taskPersist);
            agentConfig.Tasks.Add(taskHeartbeat);

            context.AgentConfigs.AddOrUpdate(ac => ac.Identifier, agentConfig);
        }
    }
}