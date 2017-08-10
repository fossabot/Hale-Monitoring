using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Hale.Core.Models.Messages;
using Microsoft.Owin.Security;
using NLog;
using System.Linq;
using Hale.Core.Model.Models;
using Hale.Core.Model.Interfaces;
using Hale.Core.Services;
using System.Collections.Generic;

namespace Hale.Core.Controllers
{
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    [RoutePrefix("api/v1/authentication")]
    public class AuthenticationController : ApiController
    {

        private readonly Logger      _log;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthenticationController() : this(new AuthService(), new UserService()) { }
        public AuthenticationController(IAuthService authService, IUserService userService)
        {
            _log = LogManager.GetCurrentClassLogger();
            _authService = authService;
            _userService = userService;
        }

        internal string _currentUsername => Request
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
            var passwordAccepted = _authService.Authorize(attempt.Username, attempt.Password);
            if (!passwordAccepted)
                return Unauthorized();

            CreateUserClaims(attempt);
            return Ok();
        }

        [Route("admin")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AdminStatus()
        {
            return Ok();
        }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <returns></returns>
        [Route()]
        [HttpGet]
        [Authorize(Roles = "User")]
        public IHttpActionResult Status()
        {
            return Ok();
        }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <returns></returns>
        [Route()]
        [HttpDelete]
        [Authorize]
        public IHttpActionResult Logout()
        {
            var context = Request.GetOwinContext();
            context.Authentication.SignOut();
            return Ok();
        }

        [Route("activate")]
        [HttpPost]
        public IHttpActionResult Activate([FromBody]ActivationAttemptDTO attempt) 
        {
            var gotActivated = _authService.Activate(attempt);

            if (!gotActivated)
                return Unauthorized(); // this is up for debate. any better suggestion? -SA 2017-08-10

            return Ok();
        }

        [Route("change-password")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult ChangePassword(PasswordDTO passwordChange)
        {
            var passwordAccepted = _authService.Authorize(_currentUsername, passwordChange.oldPassword);

            if (!passwordAccepted)
                return Unauthorized();

            _authService.ChangePassword(_currentUsername, passwordChange.newPassword);
            return Ok();
        }

        private void CreateUserClaims(LoginAttemptDTO attempt)
        {
            var user = _userService.GetUserByUserName(attempt.Username);
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "User"),
                new Claim(ClaimsIdentity.DefaultNameClaimType, attempt.Username)
            };

            if (user.IsAdmin)
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin"));

            Request
                .GetOwinContext()
                .Authentication
                .SignIn(
                    new AuthenticationProperties() { IsPersistent = attempt.Persistent },
                    new ClaimsIdentity(claims, "HaleCoreAuth")
                );
        }

    }
}
