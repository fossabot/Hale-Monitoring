using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Migrations
{
    [Migration(5)]
    public class M005CreateModulesSchema: Migration
    {
        public override void Up()
        {
            Create.Schema("Modules");
        }

        public override void Down()
        {
            Delete.Schema("Modules");
        }
    }
}
