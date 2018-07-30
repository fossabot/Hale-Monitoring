namespace Hale.Core.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;

    /// <summary>
    /// Base API Controller providing a db instance and auth protection.
    /// </summary>
    [Authorize]
    public abstract class ProtectedApiController : ControllerBase
    {
        internal string CurrentUsername => this.Request
            .HttpContext
            .User
            .Identities
            .First()
            .Claims
            .First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType)
            .Value;
    }
}
