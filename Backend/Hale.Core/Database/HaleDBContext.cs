namespace Hale.Core.Contexts
{
    using Lib.Config;
    using Models.Modules;
    using Models.Nodes;
    using Models.Users;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class HaleDBContext : DbContext
    {

        /// <summary>
        /// Default constructor for UserContext. Sets the connection string used to \"HaleDB\".
        /// </summary>
        public HaleDBContext()
            : base("name=HaleDB")
        {
        }


        #region Users
        /// <summary>
        /// A DBSet used for persisting Accounts to the UserContext.
        /// </summary>
        public virtual DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// A DBSet used for persisting AccountDetails to the UserContext.
        /// </summary>
        public virtual DbSet<AccountDetail> AccountDetails { get; set; }

        #endregion
        #region Nodes

        public virtual DbSet<Host> Hosts { get; set; }

        public virtual DbSet<HostDetail> HostDetails { get; set; }

        #endregion
        #region Modules

        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Function> Functions { get; set; }
        public virtual DbSet<Result> Results { get; set; }
        public virtual DbSet<InfoRecord> InfoRecords { get; set; }
        public virtual DbSet<CheckRecord> CheckRecords { get; set; }

        #endregion
        #region AgentConfig

        public virtual DbSet<AgentConfigSet> AgentConfigs { get; set; }

        #endregion

        /// <summary>
        /// Sets the default schema for this contextg to \"User.\"
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new HaleDBConvention());
            base.OnModelCreating(modelBuilder);
        }
    }


}