using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Hale.Core.Controllers
{
    [Authorize]
    public abstract class ProtectedApiController: ApiController
    {
    }
}
