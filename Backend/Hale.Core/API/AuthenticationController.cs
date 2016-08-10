using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using Hale.Core.Entities;
using Hale.Core.Entities.Security;
using Hale.Core.Handlers;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using NLog;
using Microsoft.Owin;
using Hale.Core.Entities.Shared;
using Hale.Core.Contexts;
using System.Linq;
using System;

namespace Hale.Core.API
{
    /// <summary>
    /// API for handling logins, logouts and status checks.
    /// </summary>
    [RoutePrefix("api/v1/authentication")]
    public class AuthenticationController : ApiController
    {

        readonly Logger _log;
        // readonly Users _users;

        
        internal AuthenticationController()
        {
            _log = LogManager.GetCurrentClassLogger();
           // _users = new Users();
        }

        private LoginResponse DoLogin(string username, string password, bool persistent = false)
        {
            try
            {
                using (var db = new HaleDBModel())
                {
                    var user = db.Users.FirstOrDefault(x => x.UserName == username);
                    var passwordAccepted = BCrypt.Net.BCrypt.Verify(password, user.Password);

                    if (passwordAccepted)
                    {
                        var context = Request.GetOwinContext();
                        context.Authentication.SignIn(
                            new AuthenticationProperties()
                            {
                                IsPersistent = persistent,
                            },
                            new ClaimsIdentity(
                                new[] { new Claim(ClaimsIdentity.DefaultNameClaimType, username) },
                                "HaleCoreAuth"
                            )
                        );

                        return new LoginResponse()
                        {
                            UserId = user.Id,
                            Error = ""
                        };
                    }
                    else
                        throw new Exception();
                }
            }
            catch
            {
                _log.Info("Authorization failed - " + Request.GetOwinContext().Request.RemoteIpAddress + "@'" + username + "'");
                return new LoginResponse()
                {
                    UserId = -1,
                    Error = "Invalid credentials"
                };
            }
            
        }


        /// <summary>
        /// Creates claim and session if the authentication succeeds. 
        /// </summary>
        /// <param name="auth">A JSON Serialized authentication attempt.</param>
        /// <returns>A custom LoginResponse that will be stored in the local storage for the Hale-GUI ember application.</returns>
        [Route("login")]
        [HttpPost]
        public LoginResponse Login([FromBody]Authentication auth)
        {
            _log.Info(JsonConvert.SerializeObject(auth));
            return DoLogin(auth.Username, auth.Password);
        }


        /// <summary>
        /// Check whether the current session is authenticated or not.
        /// </summary>
        /// <returns>A statuscode and an error message, in case there happens to be one.</returns>
        [Route("status")]
        [Route()]
        [HttpGet]
        public HttpResponseMessage Status()
        {
            IOwinContext context = Request.GetOwinContext();
            bool authenticated = (context.Authentication.User != null);

            return new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    error = "",
                    authenticated = authenticated
                })),
                StatusCode = (authenticated ? HttpStatusCode.OK : HttpStatusCode.Unauthorized)

            };
        }


        /// <summary>
        /// Deletes claim and session if there is any. 
        /// </summary>
        /// <returns>200</returns>
        [Route()]
        [HttpDelete]
        public HttpResponseMessage Logout()
        {
            var context = Request.GetOwinContext();
            context.Authentication.SignOut();
            return new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    error = "",
                    authenticated = false
                })),
                StatusCode = HttpStatusCode.OK

            };
        }
    }
}
