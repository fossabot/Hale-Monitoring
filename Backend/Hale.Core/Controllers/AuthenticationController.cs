using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Hale.Core.Models.Messages;
using Hale.Core.Models.Users;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using NLog;
using Microsoft.Owin;
using Hale.Core.Models.Shared;
using Hale.Core.Contexts;
using System.Linq;

namespace Hale.Core.Controllers
{
    [RoutePrefix("api/v1/authentication")]
    public class AuthenticationController : ApiController
    {

        #region Constructors and declarations
        private readonly Logger      _log;
        private readonly HaleDBContext _db;

        internal AuthenticationController() : this(new HaleDBContext()) { }

        public AuthenticationController(HaleDBContext context)
        {
            _log = LogManager.GetCurrentClassLogger();
            _db = context;
        }
        #endregion
        #region Private Methods

        private void CreateUserClaims(LoginAttempt attempt)
        {
            Request
                .GetOwinContext()
                .Authentication
                .SignIn(
                    new AuthenticationProperties()
                    {
                        IsPersistent = attempt.Persistent
                    },
                    new ClaimsIdentity(
                        new[] {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "User"),
                new Claim(ClaimsIdentity.DefaultNameClaimType, attempt.Username)
                        }, "HaleCoreAuth")
                    );
        }

        #endregion

        [Route("")]
        [HttpPost]
        private IHttpActionResult Login([FromBody] LoginAttempt attempt)
        {
            var user = _db.Accounts.First(x => x.UserName == attempt.Username);
            if (user == null)
                return Unauthorized();

            var passwordAccepted = BCrypt.Net.BCrypt.Verify(attempt.Password, user.Password);

            if (!passwordAccepted)
                return Unauthorized();

            else {
                CreateUserClaims(attempt);
                return Ok();
            }
        }

        [Route()]
        [HttpGet]
        [Authorize(Roles = "User")]
        public IHttpActionResult Status()
        {
            return Ok();
        }

        [Route()]
        [HttpDelete]
        [Authorize]
        public IHttpActionResult Logout()
        {
            var context = Request.GetOwinContext();
            context.Authentication.SignOut();
            return Ok();
        }



    }
}
