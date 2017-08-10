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

        [HttpGet]
        [Route("available")]
        public IHttpActionResult CheckIfAvailable(string username)
        {
            
            return Ok(_userService.GetUsernameAvailable(username));
        }

        /// <summary>
        /// Get details about a specific user by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        [Route("")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Create([FromBody] CreateAccountRequestDTO userRequest)
        {
            var currentUser = _userService.GetUserByUserName(_currentUsername);
            _userService.CreateUser(userRequest, currentUser);
            return Ok();
        }

        

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{id}")]
        public IHttpActionResult Update(int id, [FromBody]UserDTO user)
        {
            _userService.UpdateUser(id, user, _currentUsername);
            return Ok();
        }

        
    }
}
