using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Model.Exceptions
{
    public class HaleUnauthorizedException : Exception
    {
        public HaleUnauthorizedException() : base() { }
        public HaleUnauthorizedException(string text) : base(text) { }
    }
}
