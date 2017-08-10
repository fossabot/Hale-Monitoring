using Hale.Core.Model.Exceptions;
using Hale.Core.Model.Interfaces;
using Hale.Core.Model.Models;
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

            if (user == null || !user.Activated || !user.Enabled)
                return false;

            return ValidatePassword(user.Password, password);
        }

        public bool Activate(ActivationAttemptDTO attempt)
        {
            var user = _db.Accounts.FirstOrDefault(x => x.UserName == attempt.Username);

            if (user == null || user.Activated || !user.Enabled)
                return false;
            if (!ValidatePassword(user.Password, attempt.ActivationPassword))
                return false; 

            user.Activated = true;
            user.Password = CreateHash(attempt.NewPassword);
            _db.SaveChanges();

            return true;
        }

        public void ChangePassword(string userName, string newPassword)
        {
            _db.Accounts.FirstOrDefault(x => x.UserName == userName).Password = CreateHash(newPassword);
            _db.SaveChanges();
        }

        private bool ValidatePassword(string hash, string attempt)
        {
            return BCrypt.Net.BCrypt.Verify(attempt, hash);
        }

        private string CreateHash(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text, 5);
        }

       
    }
}
