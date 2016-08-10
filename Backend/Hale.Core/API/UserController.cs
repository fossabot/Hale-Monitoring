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
        private readonly Logger _log;

        internal UsersController()
        {
            _log = LogManager.GetCurrentClassLogger();   
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
        // [Authorize]
        [Route("")]
        [ResponseType(typeof(User))]
        [AcceptVerbs("POST")]
        public IHttpActionResult Add([FromBody] NewUserRequest userRequest)
        {
            // Using a non-inherited class containing only the attributes needed seem to be the only sane way
            // of doing this as we dont want to expose all attributes (like enabled or active) in the API.
            // -SA 2016-08-11

            using (var db = new HaleDBModel())
            {
                if (db.Users.Where(x => x.UserName == userRequest.UserName).Any())
                    return UserAlreadyExistsResponse();
                else
                {

                    var user = new User();

                    user.UserName = userRequest.UserName;
                    user.Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password, 5);
                    user.FullName = userRequest.FullName;

                    db.Users.Add(user);
                    db.SaveChanges();

                    return Ok(db.Users.First(x => x.UserName == user.UserName));
                }
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
            using (var db = new HaleDBModel())
            {
                if (db.Users.Find(id) == null)
                    return UserNotFoundResult(id);

                else if (db.UserDetails.Any(x => x.UserId == id && x.Key == key))
                    return DetailAlreadyExistsResponse(id, key);

                else
                {
                    db.UserDetails.Add(detail);
                    db.SaveChanges();

                    return Ok(db.UserDetails.First(x => x.Id == id && x.Key == key));
                }
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
                return Ok(db.Users.ToList());
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
            using (var db = new HaleDBModel())
            {
                var user = db.Users.Find(userid);
                if (user == null)
                    return UserNotFoundResult(userid);
                else
                {
                    var detail = user.UserDetails.FirstOrDefault(x => x.Id == detailid);
                    if (detail == null)
                        return UserDetailNotFoundResult(detailid);
                    else
                    {
                        return Ok(detail);
                    }
                }
                
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
        private IHttpActionResult UserDetailNotFoundResult(int id)
        {
            return new StringResult(
                HttpStatusCode.NotFound,
                "User Detail not found",
                detail:
                    $"User Detail #{id} not found!", request: Request
                );
        }

        private IHttpActionResult DetailAlreadyExistsResponse(int id, string key)
        {
            return new StringResult(
                HttpStatusCode.PreconditionFailed,
                "User Detail already exists",
                detail:
                    $"User Detail {key} for user #{id} already exists.", request: Request
                );
        }
        private StringResult UserAlreadyExistsResponse()
        {
            return new StringResult(HttpStatusCode.PreconditionFailed, "There is already a user with that username.", Request);
        }

    }

    
}
