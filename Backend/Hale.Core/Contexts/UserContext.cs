namespace Hale.Core.Contexts
{
    using Models.User;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class UserContext : DbContext
    {

        /// <summary>
        /// Default constructor for UserContext. Sets the connection string used to \"HaleDB\".
        /// </summary>
        public UserContext()
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
        /// Sets the default schema for this contextg to \"User.\"
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("User");
            base.OnModelCreating(modelBuilder);
        }
    }


}