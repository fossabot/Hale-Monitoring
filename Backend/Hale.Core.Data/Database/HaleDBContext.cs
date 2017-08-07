namespace Hale.Core.Data.Contexts
{
    using Hale.Core.Data.Entities;
    using System.Data.Entity;

    /// <summary>
    /// TODO: Add text here.
    /// </summary>
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

        #endregion
        #region Modules

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

        #endregion
        #region AgentConfig

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public virtual DbSet<AgentConfigSet> AgentConfigs { get; set; }

        public virtual DbSet<AgentConfigSetTask> AgentConfigSetTasks { get;set;}

        public virtual DbSet<AgentConfigSetFunctionSettings> AgentConfigSetFunctionSettings { get;set; }

        public virtual DbSet<AgentConfigSetFuncSettings> AgentConfigSetFuncSettings { get;set; }

        public virtual DbSet<AgentConfigSetCheckAction> AgentConfigSetCheckActions { get;set; }



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