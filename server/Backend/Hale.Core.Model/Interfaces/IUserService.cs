namespace Hale.Core.Model.Interfaces
{
    using System.Collections.Generic;
    using Hale.Core.Model.Models;
    using Hale.Core.Models;
    using Hale.Core.Models.Users;

    public interface IUserService
    {
        UserDTO GetUserById(int id);

        UserDTO GetUserByUserName(string userName);

        IList<UserSummaryDTO> List();

        void CreateUser(CreateAccountRequestDTO newUser, UserDTO currentUser);

        void UpdateUser(int id, UserDTO user, string currentUsername);

        bool GetUsernameAvailable(string username);
    }
}
