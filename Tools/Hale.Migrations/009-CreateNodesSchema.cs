using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Migrations
{
    [Migration(9)]
    public class M009CreateNodesSchema : Migration
    {
        public override void Up()
        {
            Create.Schema("Nodes");
        }

        public override void Down()
        {
            Delete.Schema("Nodes");
        }

    }
}
