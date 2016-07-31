using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Migrations
{
    public partial class DevelopmentProfile
    {
        public void SeedUsers()
        {
            const string PWD_TESTING = "$2a$08$RdLn/nkBKpNKpxRDTBpMp.fdbcf8hkUmR7/bfaDjfOWevgR2mdOvW"; // test01

            for (int i = 1; i <= 3; i++)
            {
                Insert.IntoTable("Accounts").InSchema("Users")
                    .Row(new
                    {
                        Fullname = $"Development User {i}",
                        Username = $"devuser{i}",
                        Password = PWD_TESTING,
                        OldPassword = PWD_TESTING,
                        Created = DateTime.Now,
                        Changed = DateTime.Now,
                        Activated = true
                    })
                ;
                Insert.IntoTable("Emails").InSchema("Users")
                    .Row(new
                    {
                        // NOTE: This will only work if a full rollback has been done before migrating -NM 2016-07-31
                        AccountId = i, 
                        Address = $"user{i}@hale.dev",
                        Primary = true
                    })
                ;
            }
        }
    }
}
