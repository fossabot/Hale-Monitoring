using Hale.Core.Controllers;
using Hale.Core.Models.Shared;
using Hale.Core.Models.Users;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale_Core_UnitTests.Controllers
{
    public class AuthenticationControllerTests
    {
        UserContextHelper helper = new UserContextHelper();

        [TestCase]
        public void Auth_LoginTest()
        {
            // How do we test this?
            // We need to either supply an Owin Context or start an Owin Server in test context.

            
        }
    }
}
