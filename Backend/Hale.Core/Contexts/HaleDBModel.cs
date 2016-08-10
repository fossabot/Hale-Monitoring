namespace Hale.Core.Contexts
{
    using Entities.Security;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class HaleDBModel : DbContext
    {
        // Your context has been configured to use a 'HaleDBModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Hale.Core.Contexts.HaleDBModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'HaleDBModel' 
        // connection string in the application configuration file.
        public HaleDBModel()
            : base("name=HaleDB")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserDetail> UserDetails { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}