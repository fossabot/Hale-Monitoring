namespace Hale.Core.Services
{
    using System.Linq;
    using Hale.Core.Model.Interfaces;
    using Hale.Core.Model.Models;

    public class AuthService : HaleBaseService, IAuthService
    {
        public bool Authorize(string username, string password)
        {
            var user = this.Db.Accounts.FirstOrDefault(x => x.UserName == username);

            if (user == null || !user.Activated || !user.Enabled)
            {
                return false;
            }

            return this.ValidatePassword(user.Password, password);
        }

        public bool Activate(ActivationAttemptDTO attempt)
        {
            var user = this.Db.Accounts.FirstOrDefault(x => x.UserName == attempt.Username);

            if (user == null || user.Activated || !user.Enabled)
            {
                return false;
            }

            if (!this.ValidatePassword(user.Password, attempt.ActivationPassword))
            {
                return false;
            }

            user.Activated = true;
            user.Password = this.CreateHash(attempt.NewPassword);
            this.Db.SaveChanges();

            return true;
        }

        public void ChangePassword(string userName, string newPassword)
        {
            this.Db.Accounts.FirstOrDefault(x => x.UserName == userName).Password = this.CreateHash(newPassword);
            this.Db.SaveChanges();
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
