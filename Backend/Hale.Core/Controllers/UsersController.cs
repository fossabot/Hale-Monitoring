using System;
using System.Net.Http;
using System.Web.Http;
using Hale.Core.Models.Users;
using NLog;
using System.Linq;
using System.Security.Claims;
using Hale.Core.Data.Entities;
using Hale.Core.Model.Interfaces;
using Hale.Core.Services;
using Hale.Core.Model.Models;

namespace Hale.Core.Controllers
{
    [RoutePrefix("api/v1/users")]
    public class UsersController : ProtectedApiController
    {

        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly IUserService _userService;

        public UsersController() : this(new UserService()) { }
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get details about a specific user by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Get information about the currently logged in user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("current")]
        public IHttpActionResult GetCurrent()
        {
            var currentUser = _userService.GetUserByUserName(_currentUsername);
            return Ok(currentUser);
        }

        /// <summary>
        /// Get a list of users with reduced user details.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("")]
        public IHttpActionResult List()
        {
            var userList = _userService.List();
            return Ok(userList);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [Authorize, HttpPost, Route("")]
        public IHttpActionResult Create([FromBody] CreateAccountRequestDTO userRequest)
        {
            _userService.CreateUser(userRequest);
            return Ok();
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize, HttpPatch, Route("{id}")]
        public IHttpActionResult Update(int id, [FromBody]UserDTO user)
        {
            _userService.UpdateUser(id, user, _currentUsername);
            return Ok();
        }
    }
}
