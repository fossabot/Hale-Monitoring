using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Migrations
{
    [Migration(4)]
    public class M004CreateUsersPhonesTable : Migration
    {
        public override void Up()
        {
            Create.Table("Phones").InSchema("Users")
                .WithColumn("Id").AsInt32().NotNullable()
                    .PrimaryKey().Identity()
                .WithColumn("AccountId").AsInt32().NotNullable()
                    .ForeignKey("FK_Phones_AccountId_Accounts_Id", "Users", "Accounts", "Id")
                        .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumn("Number").AsString().NotNullable().Unique()
                .WithColumn("Primary").AsBoolean().NotNullable()
            ;

            // One option here would be to add a composite unique key constraint on AccountId and Primary,
            // but as we want the user to be able to have multiple secondary phone numbers, that is not an option.
            // The constraint should be implemented in the core instead.

            // -SA 2016-07-27

            Create.Index("IX_Phones_AccountId")
                .OnTable("Phones").InSchema("Users").OnColumn("AccountId");
            ;
        }

        public override void Down()
        {
            Delete.Index("IX_Phones_AccountId")
                .OnTable("Phones").InSchema("Users");

            Delete.Table("Phones").InSchema("Users");
        }
    }
}
