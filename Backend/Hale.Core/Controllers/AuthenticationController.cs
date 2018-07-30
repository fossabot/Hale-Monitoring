namespace Hale.Core.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Web.Http;
    using Hale.Core.Model.Interfaces;
    using Hale.Core.Model.Models;
    using Hale.Core.Models.Messages;
    using Hale.Core.Services;
    using Microsoft.Owin.Security;
    using NLog;

    /// <summary>
    /// TODO: Add text here
    /// </summary>
    [RoutePrefix("api/v1/authentication")]
    public class AuthenticationController : ApiController
    {
        private readonly Logger log;
        private readonly IAuthService authService;
        private readonly IUserService userService;

        public AuthenticationController()
            : this(new AuthService(), new UserService())
        {
        }

        public AuthenticationController(IAuthService authService, IUserService userService)
        {
            this.log = LogManager.GetCurrentClassLogger();
            this.authService = authService;
            this.userService = userService;
        }

        internal string CurrentUsername => this.Request
            .GetOwinContext()
            .Authentication
            .User
            .Identities
            .First()
            .Claims
            .First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType)
            .Value;

        /// <summary>
        /// Creates a claim on successful sign in.
        /// </summary>
        /// <param name="attempt"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public IHttpActionResult Login([FromBody] LoginAttemptDTO attempt)
        {
            var passwordAccepted = this.authService.Authorize(attempt.Username, attempt.Password);
            if (!passwordAccepted)
            {
                return this.Unauthorized();
            }

            this.CreateUserClaims(attempt);
            return this.Ok();
        }

        [Route("admin")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AdminStatus()
        {
            return this.Ok();
        }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <returns></returns>
        [Route]
        [HttpGet]
        [Authorize(Roles = "User")]
        public IHttpActionResult Status()
        {
            return this.Ok();
        }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <returns></returns>
        [Route]
        [HttpDelete]
        [Authorize]
        public IHttpActionResult Logout()
        {
            var context = this.Request.GetOwinContext();
            context.Authentication.SignOut();
            return this.Ok();
        }

        [Route("activate")]
        [HttpPost]
        public IHttpActionResult Activate([FromBody]ActivationAttemptDTO attempt)
        {
            var gotActivated = this.authService.Activate(attempt);

            if (!gotActivated)
            {
                return this.Unauthorized(); // this is up for debate. any better suggestion? -SA 2017-08-10
            }

            return this.Ok();
        }

        [Route("change-password")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult ChangePassword(PasswordDTO passwordChange)
        {
            var passwordAccepted = this.authService.Authorize(this.CurrentUsername, passwordChange.OldPassword);

            if (!passwordAccepted)
            {
                return this.Unauthorized();
            }

            this.authService.ChangePassword(this.CurrentUsername, passwordChange.NewPassword);
            return this.Ok();
        }

        private void CreateUserClaims(LoginAttemptDTO attempt)
        {
            var user = this.userService.GetUserByUserName(attempt.Username);
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "User"),
                new Claim(ClaimsIdentity.DefaultNameClaimType, attempt.Username)
            };

            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin"));
            }

            this.Request
                .GetOwinContext()
                .Authentication
                .SignIn(
                    new AuthenticationProperties() { IsPersistent = attempt.Persistent },
                    new ClaimsIdentity(claims, "HaleCoreAuth"));
        }
    }
}
