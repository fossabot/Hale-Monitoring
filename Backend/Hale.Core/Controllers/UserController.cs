using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Hale.Core.Models;
using Hale.Core.Models.User;
using Hale.Core.Handlers;
using Hale.Core.Utils;
using NLog;
using Hale.Core.Contexts;
using System.Linq;
using System.Data.Entity;

namespace Hale.Core.Controllers
{

    /// <summary>
    /// API for passing user data.
    /// </summary>
    [RoutePrefix("api/v1/users")]
    public class UsersController : ApiController
    {
        private readonly Logger _log;
        public readonly UserContext db;

        public UsersController() : this(new UserContext())
        {
            _log = LogManager.GetCurrentClassLogger();
        }

        public UsersController(UserContext context)
        {
            db = context;
            _log = LogManager.GetCurrentClassLogger();   
        }

        /// <summary>
        /// Fetch a specific user record. (Auth)
        /// </summary>
        /// <param name="id">The user id to fetch.</param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        [ResponseType(typeof(Account))]
        public IHttpActionResult Get(int id)
        {

            var user = db.Accounts.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return UserNotFoundResult(id);
            else
                return Ok(user);
        }



        /// <summary>
        /// List user records from the database. (Auth)
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        [Route()]
        [ResponseType(typeof(List<Account>))]
        [AcceptVerbs("GET")]
        public IHttpActionResult List()
        {
            return Ok(db.Accounts.ToList());
        }


        /// <summary>
        /// Create a new user. (Auth)
        /// </summary>
        /// <param name="user">A user model instance.</param>
        /// <returns></returns>
        // [Authorize]
        [Route("")]
        [ResponseType(typeof(Account))]
        [AcceptVerbs("POST")]
        public IHttpActionResult Add([FromBody] CreateAccountRequest userRequest)
        {
            // Using a non-inherited class containing only the attributes needed seem to be the only sane way
            // of doing this as we dont want to expose all attributes (like enabled or active) in the API.
            // -SA 2016-08-11

            if (db.Accounts.Where(x => x.UserName == userRequest.UserName).Any())
                return UserAlreadyExistsResponse();
            else
            {

                var user = new Account();

                user.UserName = userRequest.UserName;
                user.Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password, 5);
                user.FullName = userRequest.FullName;

                db.Accounts.Add(user);
                db.SaveChanges();

                return Ok(db.Accounts.First(x => x.UserName == user.UserName));
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
        [ResponseType(typeof(Account))]
        public IHttpActionResult Update(int id, [FromBody]Account user)
        {
            try {
                db.Accounts.Attach(user);
                db.SaveChanges();

                return Ok(db.Accounts.First(x => x.Id == id));

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
        /// <param name="detail">The full accountdetails object.</param>
        /// <returns></returns>
        [Authorize]
        [Route("{id}/details/{key}")]
        [ResponseType(typeof(AccountDetail))]
        [AcceptVerbs("POST")]
        public IHttpActionResult AddDetail(int id, string key, [FromBody] AccountDetail detail)
        {
            if (db.Accounts.Find(id) == null)
                return UserNotFoundResult(id);

            else if (db.AccountDetails.Any(x => x.UserId == id && x.Key == key))
                return DetailAlreadyExistsResponse(id, key);

            else
            {
                db.AccountDetails.Add(detail);
                db.SaveChanges();

                return Ok(db.AccountDetails.First(x => x.Id == id && x.Key == key));
            }
        }



        /// <summary>
        /// List user details for a specific user. (Auth)
        /// </summary>
        /// <param name="id">The user id to retreive the details for.</param>
        /// <returns></returns>
        //[Authorize]
        [AcceptVerbs("GET")]
        [ResponseType(typeof(List<AccountDetail>))]
        [Route("{id}/details")]
        public IHttpActionResult Details(int id)
        {
            var user = db.Accounts.Include("AccountDetails").FirstOrDefault(u => u.Id == id);

            if (user == null)
                return UserNotFoundResult(id);

            var accountDetails = user.AccountDetails.ToList();

            return Ok(accountDetails);
        }

        /// <summary>
        /// Get a specific user detail for a specified user id. (Auth)
        /// </summary>
        /// <param name="userid">The user id to retreive the detail for.</param>
        /// <param name="detailid">The detail id to fetch.</param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs("GET")]
        [ResponseType(typeof(AccountDetail))]
        [Route("{userid}/details/{detailid}")]
        // GET: /api/user/{id}/detail/{key}
        public IHttpActionResult Detail(int userid, int detailid)
        {
            var user = db.Accounts.Find(userid);
            if (user == null)
                return UserNotFoundResult(userid);
            else
            {
                var detail = user.AccountDetails.FirstOrDefault(x => x.Id == detailid);
                if (detail == null)
                    return AccountDetailNotFound(detailid);
                else
                {
                    return Ok(detail);
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
        private IHttpActionResult AccountDetailNotFound(int id)
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
