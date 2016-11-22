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

namespace Hale.Core.Controllers
{
    
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    [RoutePrefix("api/v1/users")]
    public class UsersController : ApiController
    {
        #region Constructors and declarations
        private readonly Logger _log;
        private readonly HaleDBContext db;

        internal UsersController() : this(new HaleDBContext()) { }
        internal UsersController(HaleDBContext context)
        {
            db = context;
            _log = LogManager.GetCurrentClassLogger();   
        }
        #endregion
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize, HttpGet, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var user = db.Accounts.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <returns></returns>
        [Authorize, HttpGet, Route("")]
        public IHttpActionResult List()
        {
            return Ok(db.Accounts.ToList());
        }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [Authorize, HttpPost, Route("")]
        public IHttpActionResult Create([FromBody] CreateAccountRequest userRequest)
        {
            if (db.Accounts.Any(x => x.UserName == userRequest.UserName))
                return InternalServerError();
            else
            {
                var user = new Account
                {
                    UserName = userRequest.UserName,
                    Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password, 5),
                    FullName = userRequest.FullName
                };

                db.Accounts.Add(user);
                db.SaveChanges();

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
                db.Accounts.Attach(user);
                db.SaveChanges();

                return Ok();
            }
            catch (Exception x)
            {
                return InternalServerError(x);
            }
        }
    }
}
