namespace Hale.Core.Data.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Runtime.InteropServices;
    using Hale.Core.Data.Entities.Agent;
    using Hale.Core.Data.Entities.Modules;
    using Hale.Core.Data.Entities.Nodes;
    using Hale.Core.Data.Entities.Users;
    using Microsoft.EntityFrameworkCore;
    using Semver;
    using EModule = Entities.Modules.Module;

    /// <summary>
    /// TODO: Add text here.
    /// </summary>
    public class HaleDBContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HaleDBContext"/> class.
        /// Default constructor for UserContext. Sets the connection string used to \"HaleDB\".
        /// </summary>
        public HaleDBContext()
        {
        }

        /// <summary>
        /// A DBSet used for persisting Accounts to the UserContext.
        /// </summary>
        public virtual DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// A DBSet used for persisting AccountDetails to the UserContext.
        /// </summary>
        public virtual DbSet<AccountDetail> AccountDetails { get; set; }

        /// <summary>
        /// TODO: Add text here.
        /// </summary>
        public virtual DbSet<Node> Nodes { get; set; }

        /// <summary>
        /// TODO: Add text here.
        /// </summary>
        public virtual DbSet<NodeDetail> NodeDetails { get; set; }

        /// <summary>
        /// TODO: Add text here.
        /// </summary>
        public virtual DbSet<NodeComment> NodeComments { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public virtual DbSet<Module> Modules { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public virtual DbSet<Function> Functions { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public virtual DbSet<Result> Results { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public virtual DbSet<InfoRecord> InfoRecords { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public virtual DbSet<CheckRecord> CheckRecords { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public virtual DbSet<AgentConfigSet> AgentConfigs { get; set; }

        public virtual DbSet<AgentConfigSetTask> AgentConfigSetTasks { get; set; }

        public virtual DbSet<AgentConfigSetFunctionSettings> AgentConfigSetFunctionSettings { get; set; }

        public virtual DbSet<AgentConfigSetFunction> AgentConfigSetFuncSettings { get; set; }

        public virtual DbSet<AgentConfigSetCheckAction> AgentConfigSetCheckActions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Conventions.Add(new HaleDBConvention());

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(a => a.UserName).IsUnique();
                entity.HasData(
                new Account
                {
                    Id = 1,
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
                    Id = 2,
                    UserName = "test02",
                    Email = "nils@piksel.se",
                    FullName = "Test User 02",
                    Password = BCrypt.Net.BCrypt.HashPassword("test02", 5),
                    Enabled = true,
                    Activated = true,
                },
                new Account
                {
                    Id = 3,
                    UserName = "test03",
                    Email = "foo@bar.com",
                    FullName = "Test User 03",
                    Password = BCrypt.Net.BCrypt.HashPassword("test03", 5),
                    Enabled = true,
                    Activated = false,
                },
                new Account
                {
                    Id = 4,
                    UserName = "test04",
                    Email = "foobar@acmegroup.com",
                    FullName = "Test User 04",
                    Password = BCrypt.Net.BCrypt.HashPassword("test04", 5),
                    Enabled = false,
                    Activated = true,
                });
            });

            var now = DateTime.UtcNow;

            modelBuilder.Entity<Node>().HasData(
                new Node
                {
                    Id = 1,
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
                    Id = 2,
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
                    Id = 3,
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
                     Id = 4,
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
                    Id = 5,
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
                    Id = 6,
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
                     Id = 7,
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
                    Id = 8,
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
                    Id = 9,
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
                     Id = 10,
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
                    Id = 11,
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
                    Id = 12,
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
                    Id = 13,
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

            var agentConfig = AgentConfigSet.Empty;

            modelBuilder.Entity<EModule>().HasData(
                new EModule()
                {
                    Id = 1,
                    Identifier = "com.itshale.core.memory",
                    Version = new SemVersion(1, 0, 0),
                });

            var acf = new
            {
                AgentConfigSetId = agentConfig.Id,
                Id = 1,
                CriticalThreshold = 1.0f,
                WarningThreshold = 0.5f,
                Enabled = true,
                Function = "default",
                ModuleId = 1,
                Startup = true,
                Interval = TimeSpan.FromMinutes(10),
                Type = Lib.Modules.ModuleFunctionType.Check
            };

            agentConfig.Identifier = "seed_config01";
            agentConfig.Id = 1;

            agentConfig.Functions = null;
            //agentConfig.Functions.Add(acf);

            agentConfig.Tasks = null;

            //agentConfig.Tasks.Add(taskUpload);
            //agentConfig.Tasks.Add(taskPersist);
            //agentConfig.Tasks.Add(taskHeartbeat);

            modelBuilder.Entity<AgentConfigSetFunctionSettings>().HasData(
                new AgentConfigSetFunctionSettings()
                {
                    Id = 1,
                    AgentConfigSetFunctionId = acf.Id,
                    Target = "default",
                    Key = "foo",
                    Value = "bar"
                });

            modelBuilder.Entity<AgentConfigSetFunction>().HasData(acf);

            modelBuilder.Entity<AgentConfigSet>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasIndex(a => a.Identifier).IsUnique();
                entity.HasData(agentConfig);
                /*
                entity.OwnsOne(a => a.Tasks).HasData(
                    new
                    {
                        Capacity = 1,
                        AgentConfigSetId = 2,
                        Id = 1,
                        Enabled = true,
                        Startup = true,
                        Interval = TimeSpan.FromMinutes(5),
                        Name = "uploadResults"
                    },
                    new
                    {
                        Capacity = 1,
                        AgentConfigSetId = 3,
                        Id = 2,
                        Enabled = true,
                        Startup = true,
                        Interval = TimeSpan.FromMinutes(2),
                        Name = "persistResults",
                    },
                    new
                    {
                        Capacity = 1,
                        AgentConfigSetId = 4,
                        Id = 3,
                        Enabled = true,
                        Startup = true,
                        Interval = TimeSpan.FromMinutes(1),
                        Name = "sendHeartbeat",
                    });
                    */
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.UseSqlite("Data Source=blogging.db");
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=HaleDBTest;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}