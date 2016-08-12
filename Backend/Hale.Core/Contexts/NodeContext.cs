namespace Hale.Core.Contexts
{
    using Models.Nodes;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class NodeContext : DbContext
    {
        public NodeContext() : base("name=HaleDB") { }

        public virtual DbSet<Host> Hosts
        {
            get;
            set;
        }

        public virtual DbSet<HostDetail> HostDetails
        {
            get;
            set;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Node");
            base.OnModelCreating(modelBuilder);
        }
    }

   
}