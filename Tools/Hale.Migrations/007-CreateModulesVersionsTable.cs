using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Migrations
{
    [Migration(7)]
    public class M007CreateModulesVersionsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Versions").InSchema("Modules")
                .WithColumn("Id").AsInt32().NotNullable()
                    .PrimaryKey().Identity()
                .WithColumn("Identifier").AsString().NotNullable()
                .WithColumn("VersionMajor").AsInt16().NotNullable()
                .WithColumn("VersionMinor").AsInt16().NotNullable()
                .WithColumn("VersionPatch").AsInt16().NotNullable()
                .WithColumn("VersionBuild").AsInt16().NotNullable()
            ;
        }

        public override void Down()
        {
            Delete.Table("Versions").InSchema("Modules");
        }
    }
}
