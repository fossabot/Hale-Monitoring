using Hale.Core.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Hale.Core.Controllers
{
    /// <summary>
    /// Base API Controller providing a db instance and auth protection.
    /// </summary>
    [Authorize]
    public abstract class ProtectedApiController : ApiController
    {
        protected readonly HaleDBContext _db;

        internal ProtectedApiController() : this(new HaleDBContext()) { }
        internal ProtectedApiController(HaleDBContext context)
        {
            _db = context;
        }
    }
}
