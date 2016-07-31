using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Migrations 
{
    [Migration(6)]
    public class M006CreateModulesFunctionsTable : Migration
    {
        public override void Up()
        {
            Create.Table("FunctionTypes").InSchema("Modules")
                .WithColumn("Id").AsByte().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString().NotNullable().Unique()
            ;

            Create.Table("Functions").InSchema("Modules")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                    .ForeignKey("FK_FunctionTypes_Id_Functions_Type", "Modules", "FunctionTypes", "Id")
            ;
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_FunctionTypes_Id_Functions_Type")
                .OnTable("Functions").InSchema("Modules");
            Delete.Table("Functions").InSchema("Modules");
            Delete.Table("FunctionTypes").InSchema("Modules");
        }
    }
}
