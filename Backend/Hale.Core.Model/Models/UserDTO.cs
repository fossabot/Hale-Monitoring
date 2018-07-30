namespace Hale.Core.Model.Models
{
    using System;
    using System.Collections.Generic;
    using Hale.Core.Data.Entities.Users;

    public class UserDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public bool Activated { get; set; }

        public bool Enabled { get; set; }

        public DateTimeOffset Created { get; set; }

        public Account CreatedBy { get; set; }

        public DateTimeOffset? Modified { get; set; }

        public Account ModifiedBy { get; set; }

        public IList<AccountDetail> AccountDetails { get; set; }

        public string UserName { get; set; }

        public bool IsAdmin { get; set; }
    }
}
