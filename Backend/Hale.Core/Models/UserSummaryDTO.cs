using Hale.Core.Models.Users;

namespace Hale.Core.Models
{
    public class UserSummaryDTO
    {
        /// <summary>
        /// Convert an entity Account to a UserSummaryDTO
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static UserSummaryDTO FromAccount (Account account)
        {
            if (account == null)
                return default(UserSummaryDTO);

            return new UserSummaryDTO
            {
                Id = account.Id,
                FullName = account.FullName,
                Email = account.Email,
            };
        }

        public int Id { get; internal set; }
        public string FullName { get; internal set; }
        public string Email { get; internal set; }
    }

}