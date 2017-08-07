using Hale.Core.Data.Contexts;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Hale.Core.Controllers
{
    /// <summary>
    /// Base API Controller providing a db instance and auth protection.
    /// </summary>
    [Authorize]
    public abstract class ProtectedApiController : ApiController
    {
        internal string _currentUsername => Request
            .GetOwinContext()
            .Authentication
            .User
            .Identities
            .First()
            .Claims
            .First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType)
            .Value;
    }
}
