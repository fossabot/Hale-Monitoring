using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Models.Users
{
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public class CreateAccountRequest
    {
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// TODO: Add text here
        /// </summary>
        public string FullName { get; set; }
    }
}
