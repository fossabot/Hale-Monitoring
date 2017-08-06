using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Hale.Core.Models;
using Hale.Core.Models.Users;
using Hale.Core.Handlers;
using Hale.Core.Utils;
using NLog;
using Hale.Core.Contexts;
using System.Linq;
using System.Data.Entity;
using System.Security.Claims;

namespace Hale.Core.Controllers
{
    
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    [RoutePrefix("api/v1/users")]
    public class UsersController : ProtectedApiController
    {

        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize, HttpGet, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var user = _db.Accounts.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Get information about the currently logged in user
        /// </summary>
        /// <returns></returns>
        [Authorize, HttpGet, Route("current")]
        public IHttpActionResult GetCurrent()
        {
            var user = _db.Accounts.Single(x => x.UserName == _currentUsername);
            return Ok(new
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
            });
        }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <returns></returns>
        [Authorize, HttpGet, Route("")]
        public IHttpActionResult List()
        {
            return Ok(_db.Accounts.ToList());
        }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [Authorize, HttpPost, Route("")]
        public IHttpActionResult Create([FromBody] CreateAccountRequest userRequest)
        {
            if (_db.Accounts.Any(x => x.UserName == userRequest.UserName))
                return InternalServerError();
            else
            {
                var user = new Account
                {
                    UserName = userRequest.UserName,
                    Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password, 5),
                    FullName = userRequest.FullName
                };

                _db.Accounts.Add(user);
                _db.SaveChanges();

                return Ok();
            }
        }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize, HttpPatch, Route("{id}")]
        public IHttpActionResult Update(int id, [FromBody]Account user)
        {
            try {
                _db.Accounts.Attach(user);
                _db.SaveChanges();

                return Ok();
            }
            catch (Exception x)
            {
                return InternalServerError(x);
            }
        }


        private string _currentUsername
        {
            get
            {
                return Request.GetOwinContext().Authentication.User.Identities.First().Claims
                    .First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }
        }
    }
}
