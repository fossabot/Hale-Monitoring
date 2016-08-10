using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Hale.Core.Entities;
using Hale.Core.Entities.Security;
using Hale.Core.Handlers;
using Hale.Core.Utils;
using NLog;
using Hale.Core.Contexts;
using System.Linq;

namespace Hale.Core.API
{

    /// <summary>
    /// API for passing user data.
    /// </summary>
    [RoutePrefix("api/v1/users")]
    public class UsersController : ApiController
    {
        private readonly SecurityHandler _security;
        private readonly Users _users;
        private readonly UserDetails _userdetails;
        private readonly Logger _log;

        internal UsersController()
        {
            _log = LogManager.GetCurrentClassLogger();   
            _security = new SecurityHandler();
            _users = new Users();
            _userdetails = new UserDetails();
        }

        /// <summary>
        /// Fetch a specific user record. (Auth)
        /// </summary>
        /// <param name="id">The user id to fetch.</param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        [ResponseType(typeof(User))]
        public IHttpActionResult Get(int id)
        {

            using (var db = new HaleDBModel())
            {
                var user = db.Users.Include("UserDetails").FirstOrDefault(u => u.Id == id);

                if (user == null)
                    return UserNotFoundResult(id);

                return Ok(user);
            }

        }


        /// <summary>
        /// Create a new user. (Auth)
        /// </summary>
        /// <param name="user">A user model instance.</param>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        [ResponseType(typeof(User))]
        [AcceptVerbs("PUT")]
        public IHttpActionResult Add([FromBody] User user)
        {


            if (_users.Get(user) == null)
            {
                try {
                    _users.Create(user);
                    user = _users.Get(user);

                    return Created($"/users/{user.Id}", user);
                }
                catch (Exception x)
                {
                    return InternalServerError(x);
                }
            }
            else
            {
                return new StringResult(HttpStatusCode.PreconditionFailed, "There is already a user with that username.", Request);
            }

        }

        /// <summary>
        /// Update user attributes except for password. (Auth)
        /// </summary>
        /// <param name="id">The User Id to update.</param>
        /// <param name="user">An instance of the user model containing the new values.</param>
        /// <returns></returns>
        [Route("{id}")]
        [AcceptVerbs("PATCH")]
        [ResponseType(typeof(User))]
        public IHttpActionResult Update(int id, [FromBody]User user)
        {
            try {
                using(var db = new HaleDBModel())
                {
                    var dbUser = db.Users.First(u => u.Id == id);
                    db.Entry(dbUser).CurrentValues.SetValues(user);
                    db.SaveChanges();
                    return Ok(dbUser);
                }

            }
            catch (Exception x)
            {
                return InternalServerError(x);
            }
        }

        /// <summary>
        /// Add a user detail to the open schema table. (Auth)
        /// </summary>
        /// <param name="id">The user ID for the detail.</param>
        /// <param name="key">The key to store the detail in.</param>
        /// <param name="detail">The full userdetail object.</param>
        /// <returns></returns>
        [Authorize]
        [Route("{id}/details/{key}")]
        [ResponseType(typeof(UserDetail))]
        [AcceptVerbs("POST")]
        public IHttpActionResult AddDetail(int id, string key, [FromBody] UserDetail detail)
        {
            User user = _users.Get(
                new Entities.Security.User() {
                    Id = id
                });

            detail.Key = key;

            if (_users.Get(user) != null)
            {
                if (_userdetails.Get(user, detail) == null)
                {
                    try {
                        _userdetails.Create(user, detail);
                        detail = _userdetails.Get(user, detail);
                        return Created($"{id}/details/{key}", detail);
                    }
                    catch(Exception x)
                    {
                        return InternalServerError(x);
                    }

                }
                else
                {
                    return ResponseMessage(new HttpResponseMessage()
                    {
                        Content = new StringContent("There is already a detail with that key."),
                        StatusCode = HttpStatusCode.PreconditionFailed
                    });
                }
            }
            else
            {
                return ResponseMessage(new HttpResponseMessage()
                {
                    Content = new StringContent("There is no user with that username."),
                    StatusCode = HttpStatusCode.PreconditionFailed
                });
            }
        }

        /// <summary>
        /// List user records from the database. (Auth)
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        [Route()]
        [ResponseType(typeof(List<User>))]
        [AcceptVerbs("GET")]
        public IHttpActionResult List()
        {
            using(var db = new HaleDBModel())
            {
                var users = db.Users.ToList();

                return Ok(users);
            }
        }

        /// <summary>
        /// List user details for a specific user. (Auth)
        /// </summary>
        /// <param name="id">The user id to retreive the details for.</param>
        /// <returns></returns>
        //[Authorize]
        [AcceptVerbs("GET")]
        [ResponseType(typeof(List<UserDetail>))]
        [Route("{id}/details")]
        public IHttpActionResult Details(int id)
        {

                using (var db = new HaleDBModel())
                {
                    var user = db.Users.Include("UserDetails").FirstOrDefault(u => u.Id == id);

                    if (user == null)
                        return UserNotFoundResult(id);

                    var userDetails = user.UserDetails.ToList();
                    //var userDetails = db.UserDetails.Where(ud => ud.UserId == id).ToList();

                    return Ok(userDetails);
                }

        }

        /// <summary>
        /// Get a specific user detail for a specified user id. (Auth)
        /// </summary>
        /// <param name="userid">The user id to retreive the detail for.</param>
        /// <param name="detailid">The detail id to fetch.</param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs("GET")]
        [ResponseType(typeof(UserDetail))]
        [Route("{userid}/details/{detailid}")]
        // GET: /api/user/{id}/detail/{key}
        public IHttpActionResult Detail(int userid, int detailid)
        {
            var user = _users.Get(new Entities.Security.User() { Id = userid });
            var detail = _userdetails.Get(user, new UserDetail() { Id = detailid });
            
            if (detail != null)
            {
                return Ok(detail);
            }
            else
            {
                return new StringResult(
                    HttpStatusCode.NotFound,
                    "UserDetail not found",
                    detail:
                        $"UserDetail \"{detailid}\" for User #{userid} not found!", request: Request
                    );
            }
        }


        private IHttpActionResult UserNotFoundResult(int id)
        {
            return new StringResult(
                HttpStatusCode.NotFound,
                "User not found",
                detail:
                    $"User #{id} not found!", request: Request
                );
        }


    }

    
}
