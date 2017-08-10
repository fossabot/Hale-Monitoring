using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Model.Models
{
    public class ActivationAttemptDTO
    {
        public string Username { get; set; }
        public string ActivationPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
