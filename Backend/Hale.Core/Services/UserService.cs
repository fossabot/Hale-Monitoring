using System;
using Hale.Core.Model.Interfaces;
using Hale.Core.Model.Models;
using System.Linq;
using Hale.Core.Models;
using System.Collections.Generic;
using Hale.Core.Models.Users;
using Hale.Core.Data.Entities;

namespace Hale.Core.Services
{
    public class UserService : HaleBaseService, IUserService
    {
        public void CreateUser(CreateAccountRequestDTO newUser, UserDTO currentUser)
        {
            if (_db.Accounts.Any(x => x.UserName == newUser.UserName))
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

            _db.Accounts.Add(user);
            _db.SaveChanges();
        }

        public UserDTO GetUserById(int id)
        {
            return _db.Accounts
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
                    CreatedBy = _db.Accounts.FirstOrDefault(a => a.Id == x.CreatedBy),
                    Modified = x.Modified,
                    ModifiedBy = _db.Accounts.FirstOrDefault(a => a.Id == x.ModifiedBy),
                    AccountDetails = _db.AccountDetails.Where(ad => ad.UserId == x.Id).ToList(),
                    IsAdmin = x.IsAdmin
                })
                .FirstOrDefault();
        }

        public UserDTO GetUserByUserName(string userName)
        {
            var userId = _db.Accounts.Single(x => x.UserName == userName).Id;
            return GetUserById(userId);
        }

        public IList<UserSummaryDTO> List()
        {
            return _db.Accounts
                .Select(x => new UserSummaryDTO
                {
                    Id = x.Id,
                    Email = x.Email,
                    UserName = x.UserName,
                    FullName = x.FullName,
                    Modified = x.Modified,
                    ModifiedBy = _db.Accounts.Select(a => new UserBasicsDTO { Id = a.Id, Email = a.Email, FullName = a.FullName }).FirstOrDefault(a => a.Id == x.ModifiedBy),
                    Created = x.Created,
                    CreatedBy = _db.Accounts.Select(a => new UserBasicsDTO { Id = a.Id, Email = a.Email, FullName = a.FullName }).FirstOrDefault(a => a.Id == x.CreatedBy),
                    Enabled = x.Enabled,
                    Activated = x.Activated
                })
                .ToList();
        }

        public void UpdateUser(int id, UserDTO user, string currentUserName)
        {
            if (user.Id != id)
                throw new ArgumentException();

            var currentUser = GetUserByUserName(currentUserName);
            var account = _db.Accounts.FirstOrDefault(x => x.Id == id);

            if (account == null)
                throw new ArgumentException();

            account.Email = user.Email;
            account.FullName = user.FullName;
            account.Modified = DateTimeOffset.UtcNow;
            account.ModifiedBy = currentUser.Id;
            account.Activated = user.Activated;
            account.AccountDetails = user.AccountDetails;
            account.Enabled = user.Enabled;
            _db.SaveChanges();
        }

        public bool GetUsernameAvailable(string username)
        {
            return !_db.Accounts.Any(x => x.UserName == username);
        }
    }
}
