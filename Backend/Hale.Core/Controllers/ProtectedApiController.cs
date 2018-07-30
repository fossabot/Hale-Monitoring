namespace Hale.Core.Controllers
{
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Web.Http;

    /// <summary>
    /// Base API Controller providing a db instance and auth protection.
    /// </summary>
    [Authorize]
    public abstract class ProtectedApiController : ApiController
    {
        internal string CurrentUsername => this.Request
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
