using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Entities.Security
{
    public class NewUserRequest
    {
        public string UserName
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public string FullName
        {
            get;
            set;
        }
    }
}
