using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Migration
{
    [Migration(1)]
    public class CreateUsersSchema : FluentMigrator.Migration
    {

        public override void Up()
        {
            Create.Schema("Users");
        }

        public override void Down()
        {
            Delete.Schema("Users");
        }
    }
}
