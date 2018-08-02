namespace Hale.Core.Services
{
    using Hale.Core.Data.Contexts;

    public abstract class HaleBaseService
    {
        private readonly HaleDBContext db;

        internal HaleBaseService()
            : this(new HaleDBContext())
        {
        }

        internal HaleBaseService(HaleDBContext context)
        {
            this.db = context;
            // this.db.Database.Log = s => System.Diagnostics.Debug.Write(s);
        }

        internal HaleDBContext Db => this.Db;
    }
}
