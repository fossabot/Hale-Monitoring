namespace Hale.Core.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using Hale.Core.Model.Interfaces;
    using Hale.Core.Model.Models;
    using Hale.Core.Models.Messages;
    using Hale.Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authentication;
    using NLog;
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// TODO: Add text here
    /// </summary>
    [Route("api/v1/authentication")]
    public class AuthenticationController : ControllerBase
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
            .HttpContext
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
        public IActionResult Login([FromBody] LoginAttemptDTO attempt)
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
        public IActionResult AdminStatus()
        {
            return this.Ok();
        }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult Status()
        {
            return this.Ok();
        }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public IActionResult Logout()
        {
            var context = this.Request.HttpContext;
            context.SignOutAsync();
            return this.Ok();
        }

        [Route("activate")]
        [HttpPost]
        public IActionResult Activate([FromBody]ActivationAttemptDTO attempt)
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
        public IActionResult ChangePassword(PasswordDTO passwordChange)
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
                .HttpContext
                .SignInAsync(
                    new ClaimsPrincipal(new ClaimsIdentity(claims, "HaleCoreAuth")),
                    new AuthenticationProperties() { IsPersistent = attempt.Persistent });
        }
    }
}
