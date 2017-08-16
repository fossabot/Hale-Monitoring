namespace Hale.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hale.Core.Data.Entities.Users;
    using Hale.Core.Model.Interfaces;
    using Hale.Core.Model.Models;
    using Hale.Core.Models;
    using Hale.Core.Models.Users;

    public class UserService : HaleBaseService, IUserService
    {
        public void CreateUser(CreateAccountRequestDTO newUser, UserDTO currentUser)
        {
            if (this.Db.Accounts.Any(x => x.UserName == newUser.UserName))
            {
                throw new ArgumentException();
            }

            var user = new Account
            {
                UserName = newUser.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password, 5),
                FullName = newUser.FullName,
                Email = newUser.Email,
                Activated = false,
                Enabled = true,
                Created = DateTimeOffset.UtcNow,
                CreatedBy = currentUser.Id
            };

            this.Db.Accounts.Add(user);
            this.Db.SaveChanges();
        }

        public UserDTO GetUserById(int id)
        {
            return this.Db.Accounts
                .Where(x => x.Id == id)
                .Select(x => new UserDTO
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    FullName = x.FullName,
                    Activated = x.Activated,
                    Enabled = x.Enabled,
                    Created = x.Created,
                    CreatedBy = this.Db.Accounts.FirstOrDefault(a => a.Id == x.CreatedBy),
                    Modified = x.Modified,
                    ModifiedBy = this.Db.Accounts.FirstOrDefault(a => a.Id == x.ModifiedBy),
                    AccountDetails = this.Db.AccountDetails.Where(ad => ad.UserId == x.Id).ToList(),
                    IsAdmin = x.IsAdmin
                })
                .FirstOrDefault();
        }

        public UserDTO GetUserByUserName(string userName)
        {
            var userId = this.Db.Accounts.Single(x => x.UserName == userName).Id;
            return this.GetUserById(userId);
        }

        public IList<UserSummaryDTO> List()
        {
            return this.Db.Accounts
                .Select(x => new UserSummaryDTO
                {
                    Id = x.Id,
                    Email = x.Email,
                    UserName = x.UserName,
                    FullName = x.FullName,
                    Modified = x.Modified,
                    ModifiedBy = this.Db.Accounts.Select(a => new UserBasicsDTO { Id = a.Id, Email = a.Email, FullName = a.FullName }).FirstOrDefault(a => a.Id == x.ModifiedBy),
                    Created = x.Created,
                    CreatedBy = this.Db.Accounts.Select(a => new UserBasicsDTO { Id = a.Id, Email = a.Email, FullName = a.FullName }).FirstOrDefault(a => a.Id == x.CreatedBy),
                    Enabled = x.Enabled,
                    Activated = x.Activated
                })
                .ToList();
        }

        public void UpdateUser(int id, UserDTO user, string currentUserName)
        {
            if (user.Id != id)
            {
                throw new ArgumentException();
            }

            var currentUser = this.GetUserByUserName(currentUserName);
            var account = this.Db.Accounts.FirstOrDefault(x => x.Id == id);

            if (account == null)
            {
                throw new ArgumentException();
            }

            account.Email = user.Email;
            account.FullName = user.FullName;
            account.Modified = DateTimeOffset.UtcNow;
            account.ModifiedBy = currentUser.Id;
            account.Activated = user.Activated;
            account.AccountDetails = user.AccountDetails;
            account.Enabled = user.Enabled;
            this.Db.SaveChanges();
        }

        public bool GetUsernameAvailable(string username)
        {
            return !this.Db.Accounts.Any(x => x.UserName == username);
        }
    }
}
