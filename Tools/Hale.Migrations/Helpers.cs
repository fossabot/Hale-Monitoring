using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Migrations
{
    public static class ICreateTableWithColumnSyntaxExtensions
    {
        public static ICreateTableWithColumnSyntax WithCreateModify(this ICreateTableWithColumnSyntax table)
        {
            return table
            .WithColumn("Created").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("CreatedBy").AsInt32().Nullable()
                .ForeignKey("", "Users", "Accounts", "Id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade)
            .WithColumn("Modified").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("ModifiedBy").AsInt32().Nullable()
                .ForeignKey("", "Users", "Accounts", "Id")
                    .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }
    }
}
