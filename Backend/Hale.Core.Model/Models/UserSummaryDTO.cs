namespace Hale.Core.Models
{
    using System;
    using Hale.Core.Data.Entities.Users;
    using Hale.Core.Model.Models;

    public class UserSummaryDTO
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public DateTimeOffset? Modified { get; set; }

        public UserBasicsDTO ModifiedBy { get; set; }

        public DateTimeOffset Created { get; set; }

        public UserBasicsDTO CreatedBy { get; set; }

        public bool Activated { get; set; }

        public bool Enabled { get; set; }

        /// <summary>
        /// Convert an entity Account to a UserSummaryDTO
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static UserSummaryDTO FromAccount(Account account)
        {
            if (account == null)
            {
                return default(UserSummaryDTO);
            }

            return new UserSummaryDTO
            {
                Id = account.Id,
                FullName = account.FullName,
                Email = account.Email,
            };
        }
    }
}