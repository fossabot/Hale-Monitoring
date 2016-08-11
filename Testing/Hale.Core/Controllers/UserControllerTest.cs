using System;
using NUnit;
using Hale.Core.Models.User;
using Hale.Core.Contexts;
using System.Linq;
using Hale.Core.Controllers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Collections.Generic;
using Moq;
using System.Data.Entity;
using System.Diagnostics;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Hale_Core_UnitTests.Controllers
{
    public class UserControllerTest
    {
        UserContextHelper helper = new UserContextHelper();
       

        [TestCase]
        // HTTP GET: /api/v1/users/:id
        public void Users_GetSingleUser()
        {
            var context = helper.GetMockUserContext();
            var controller = new UsersController(context.Object);

            var user = (controller.Get(1) as OkNegotiatedContentResult<Account>).Content;

            Assert.AreEqual(context.Object.Accounts.First(x => x.Id == 1).UserName, user.UserName);
            System.Diagnostics.Trace.WriteLine($"Got user #{user.Id}, {user.UserName}");
        }

        [TestCase]
        public void Users_GetUserList()
        {
            var context = helper.GetMockUserContext();
            var controller = new UsersController(context.Object);

            var users = (controller.List() as OkNegotiatedContentResult<List<Account>>).Content;
            Assert.IsTrue(users.Count > 0);

            foreach(var account in users)
            {
                Assert.AreEqual(context.Object.Accounts.First(x => x.Id == account.Id).UserName, account.UserName);
                Assert.AreEqual(context.Object.Accounts.First(x => x.Id == account.Id).FullName, account.FullName);
            }
        }

        [TestCase]
        public void Users_CreateNewUser()
        {
            var request = new CreateAccountRequest()
            {
                UserName = "test",
                FullName = "test testsson",
                Password = "testpass"
            };

            var context = helper.GetMockUserContext().Object;
            var controller = new UsersController(context);

            var response = (controller.Add(request) as OkNegotiatedContentResult<Account>).Content;

            Assert.AreEqual(response.UserName, request.UserName);
            
            
        }
        
        [TestCase]
        public void Users_UpdateUser()
        {
            var context = helper.GetMockUserContext().Object;
            var controller = new UsersController(context);

            var input = context.Accounts.First(x => x.Id == 1);
            Assert.AreEqual(input.UserName, "leroy");
            input.UserName = "jojo";

            var output = (controller.Update(input.Id, input) as OkNegotiatedContentResult<Account>).Content;
            Assert.AreEqual(output.UserName, "jojo");
            
        }
    }
}
