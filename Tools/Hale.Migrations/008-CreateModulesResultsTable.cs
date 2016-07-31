using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Migrations
{
    [Migration(8)]
    public class M008CreateModulesResultsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Results").InSchema("Modules")
                .WithColumn("Host").AsInt32().NotNullable() // No FK yet -NM 2016-07-31
                .WithColumn("Version").AsInt32().NotNullable()
                    .ForeignKey("", "Modules", "Versions", "Id")
                .WithColumn("Function").AsInt32().NotNullable()
                    .ForeignKey("", "Modules", "Functions", "Id")
                .WithColumn("Target").AsString().Nullable()
                .WithColumn("ExectionTime").AsDateTimeOffset().NotNullable()
                .WithColumn("Message").AsString().Nullable()
                .WithColumn("Exception").AsString().Nullable()
            ;
        }

        public override void Down()
        {
            Delete.Table("Results").InSchema("Modules");
        }
    }
}
