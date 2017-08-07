using Hale.Core.Data.Entities;
using Hale.Core.Model.Models;
using Hale.Core.Models;
using Hale.Core.Models.Users;
using System.Collections.Generic;

namespace Hale.Core.Model.Interfaces
{
    public interface IUserService
    {
        UserDTO GetUserById(int id);
        UserDTO GetUserByUserName(string userName);
        IList<UserSummaryDTO> List();
        void CreateUser(CreateAccountRequestDTO newUser);
        void UpdateUser(int id, UserDTO user, string currentUsername);
    }
}
