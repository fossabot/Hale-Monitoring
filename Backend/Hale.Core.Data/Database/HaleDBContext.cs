namespace Hale.Core.Data.Contexts
{
    using System.Data.Entity;
    using System.Runtime.InteropServices;
    using Hale.Core.Data.Entities.Agent;
    using Hale.Core.Data.Entities.Modules;
    using Hale.Core.Data.Entities.Nodes;
    using Hale.Core.Data.Entities.Users;

    /// <summary>
    /// TODO: Add text here.
    /// </summary>
    [ComVisible(false)]
    public class HaleDBContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HaleDBContext"/> class.
        /// Default constructor for UserContext. Sets the connection string used to \"HaleDB\".
        /// </summary>
        public HaleDBContext()
            : base("name=HaleDB")
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

        public virtual DbSet<AgentConfigSetFunctions> AgentConfigSetFuncSettings { get; set; }

        public virtual DbSet<AgentConfigSetCheckAction> AgentConfigSetCheckActions { get; set; }

        /// <summary>
        /// Sets the default schema for this context to "User."
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new HaleDBConvention());
            base.OnModelCreating(modelBuilder);
        }
    }
}