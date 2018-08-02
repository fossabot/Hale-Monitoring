namespace Hale.Core.Controllers
{
    using Hale.Core.Model.Interfaces;
    using Hale.Core.Model.Models;
    using Hale.Core.Models.Users;
    using Hale.Core.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NLog;

    [Route("api/v1/users")]
    public class UsersController : ProtectedApiController
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly IUserService userService;

        public UsersController()
            : this(new UserService())
        {
        }

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("available")]
        public IActionResult CheckIfAvailable(string username)
        {
            return this.Ok(this.userService.GetUsernameAvailable(username));
        }

        /// <summary>
        /// Get details about a specific user by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var user = this.userService.GetUserById(id);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.Ok(user);
        }

        /// <summary>
        /// Get information about the currently logged in user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("current")]
        public IActionResult GetCurrent()
        {
            var currentUser = this.userService.GetUserByUserName(this.CurrentUsername);
            return this.Ok(currentUser);
        }

        /// <summary>
        /// Get a list of users with reduced user details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IActionResult List()
        {
            var userList = this.userService.List();
            return this.Ok(userList);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] CreateAccountRequestDTO userRequest)
        {
            var currentUser = this.userService.GetUserByUserName(this.CurrentUsername);
            this.userService.CreateUser(userRequest, currentUser);
            return this.Ok();
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody]UserDTO user)
        {
            this.userService.UpdateUser(id, user, this.CurrentUsername);
            return this.Ok();
        }
    }
}
