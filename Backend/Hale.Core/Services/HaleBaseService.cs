using Hale.Core.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Services
{
    public abstract class HaleBaseService
    {
        protected readonly HaleDBContext _db;

        internal HaleBaseService() : this(new HaleDBContext()) { }
        internal HaleBaseService(HaleDBContext context)
        {
            _db = context;
            _db.Database.Log = s => System.Diagnostics.Debug.Write(s);
        }
    }
}
