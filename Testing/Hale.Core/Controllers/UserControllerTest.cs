using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace Hale_Core_UnitTests.Controllers
{
    [TestClass]
    public class UserControllerTest
    {

        internal List<Account> GetMockData() {
            return new List<Account>()
            {
                new Account { Id = 1, UserName = "leroy", FullName = "leroy jenkins" },
                new Account { Id = 2, UserName = "foo"  , FullName =" foobar"        }
            };
        }

        private Mock<DbSet<Account>> GetQueryableMockAccountDbSet()
        {
            var data = GetMockData();

            var mock = new Mock<DbSet<Account>>();

            mock.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
            mock.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            mock.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            mock.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mock.Setup(m => m.Add(It.IsAny<Account>())).Callback<Account>(data.Add);

         
            return mock;
        }
        
        private Mock<UserContext> GetMockUserContext()
        {
            var mock = GetQueryableMockAccountDbSet();
            var mockContext = new Mock<UserContext>();
            mockContext.Setup(x => x.Accounts).Returns(mock.Object);

            return mockContext;
        }

        [TestMethod]
        // HTTP GET: /api/v1/users/:id
        public void GetSingleUser()
        {
            var context = GetMockUserContext();
            var controller = new UsersController(context.Object);

            var user = (controller.Get(1) as OkNegotiatedContentResult<Account>).Content;

            Assert.AreEqual(context.Object.Accounts.First(x => x.Id == 1).UserName, user.UserName);
            System.Diagnostics.Trace.WriteLine($"Got user #{user.Id}, {user.UserName}");
        }

        [TestMethod]
        public void GetUserList()
        {
            var context = GetMockUserContext();
            var controller = new UsersController(context.Object);

            var users = (controller.List() as OkNegotiatedContentResult<List<Account>>).Content;
            Assert.IsTrue(users.Count > 0);

            foreach(var account in users)
            {
                Assert.AreEqual(context.Object.Accounts.First(x => x.Id == account.Id).UserName, account.UserName);
                Assert.AreEqual(context.Object.Accounts.First(x => x.Id == account.Id).FullName, account.FullName);
            }
        }

        [TestMethod]
        public void CreateNewUser()
        {
            var request = new CreateAccountRequest()
            {
                UserName = "test",
                FullName = "test testsson",
                Password = "testpass"
            };

            var context = GetMockUserContext().Object;
            var controller = new UsersController(context);

            var response = (controller.Add(request) as OkNegotiatedContentResult<Account>).Content;

            Assert.AreEqual(response.UserName, request.UserName);
            
            
        }
        
        [TestMethod]
        public void UpdateUser()
        {
            var context = GetMockUserContext().Object;
            var controller = new UsersController(context);

            var input = context.Accounts.First(x => x.Id == 1);
            Assert.AreEqual(input.UserName, "leroy");
            input.UserName = "jojo";

            var output = (controller.Update(input.Id, input) as OkNegotiatedContentResult<Account>).Content;
            Assert.AreEqual(output.UserName, "jojo");
            
        }
    }
}
