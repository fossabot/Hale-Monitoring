using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Hale.Core.Models.Users
{
    /// <summary>
    /// Corresponds to the database table Security.User
    /// </summary>
    
    public class Account
    {

        /// <summary>
        /// Corresponds to the table column User.Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Corresponds to the table column User.UserName
        /// </summary>
        [Index(IsUnique = true)]
        [StringLength(450)] // Needs a max length to be able to function as a unique field.
        public string UserName { get; set; }


        public string Email { get; set; }
        /// <summary>
        /// Corresponds to the table column User.Email
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Corresponds to the table column User.Password.
        /// Only used to hold a hashed and salted representation of the password.
        /// </summary>

        [NotMapped]
        public string PasswordInput
        {
            get;
            set;
        }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        [IgnoreDataMember]
        public string Password { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        [IgnoreDataMember]
        public string OldPassword { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        [IgnoreDataMember]
        public DateTime? PasswordChanged { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public bool Activated { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Corresponds to the table column User.Created
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;

        /// <summary>
        /// Corresponds to the table column User.CreatedBy (FK Security.Users.Id)
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Corresponds to the table column User.Changed
        /// </summary>
        public DateTime? Modified { get; set; }

        /// <summary>
        /// Corresponds to the table column User.ChangedBy (FK Security.Users.Id)
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// Aggregation of available records in the Security.AccountDetails table.
        /// Accounts [1..*] AccountDetails
        /// </summary>
        public List<AccountDetail> AccountDetails { get; set; }


    }
}
