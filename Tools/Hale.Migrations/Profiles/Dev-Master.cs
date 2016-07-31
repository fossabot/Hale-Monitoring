using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Migrations
{
    [Profile("Development")]
    public partial class DevelopmentProfile : Migration
    {
        public override void Up()
        {
            SeedUsers();
        }

        public override void Down() { }
    }
}
