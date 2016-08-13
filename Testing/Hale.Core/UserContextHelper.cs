using Hale.Core.Contexts;
using Hale.Core.Models.Users;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale_Core_UnitTests
{
    class UserContextHelper
    {
        internal List<Account> MockData
        {
            get
            {
                return new List<Account>()
            {
                new Account { Id = 1, UserName = "leroy", FullName = "leroy jenkins" },
                new Account { Id = 2, UserName = "foo"  , FullName =" foobar"        }
            };
            }
        }

        internal Mock<DbSet<Account>> GetQueryableMockAccountDbSet()
        {
            var data = MockData;

            var mock = new Mock<DbSet<Account>>();

            mock.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
            mock.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            mock.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            mock.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mock.Setup(m => m.Add(It.IsAny<Account>())).Callback<Account>(data.Add);


            return mock;
        }

        internal Mock<HaleDBContext> GetMockUserContext()
        {
            var mock = GetQueryableMockAccountDbSet();
            var mockContext = new Mock<HaleDBContext>();
            mockContext.Setup(x => x.Accounts).Returns(mock.Object);

            return mockContext;
        }
    }
}
