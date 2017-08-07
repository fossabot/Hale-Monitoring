using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Hale.Core.Data.Entities
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)] // Needs a max length to be able to function as a unique field.
        public string UserName { get; set; }

        public string Email { get; set; }
        public string FullName { get; set; }

        [NotMapped]
        public string PasswordInput
        {
            get;
            set;
        }

        [IgnoreDataMember]
        public string Password { get; set; }

        [IgnoreDataMember]
        public string OldPassword { get; set; }

        [IgnoreDataMember]
        public DateTimeOffset? PasswordChanged { get; set; }

        public bool Activated { get; set; }
        public bool Enabled { get; set; }
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
        public int? CreatedBy { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        public IList<AccountDetail> AccountDetails { get; set; }


    }
}
