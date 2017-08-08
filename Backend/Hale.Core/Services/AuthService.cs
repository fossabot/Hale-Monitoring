using Hale.Core.Model.Interfaces;
using Hale.Core.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Services
{
    public class AuthService : HaleBaseService, IAuthService
    {
        public bool Authorize(string username, string password)
        {
            var user = _db.Accounts.FirstOrDefault(x => x.UserName == username);
            if (user == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

        public void ChangePassword(string userName, string newPassword)
        {
            var user = _db.Accounts.FirstOrDefault(x => x.UserName == userName);
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, 5);

            user.Password = passwordHash;
            _db.SaveChanges();
        }
    }
}
