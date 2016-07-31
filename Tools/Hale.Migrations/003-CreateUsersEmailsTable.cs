using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
namespace Hale.Migrations
{
    [Migration(3)]
    public class M003CreateUsersEmailsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Emails").InSchema("Users")
                .WithColumn("Id").AsInt32().NotNullable()
                    .PrimaryKey().Identity()
                .WithColumn("AccountId").AsInt32().NotNullable()
                    .ForeignKey("FK_Accounts_Id_Emails_AccountId", "Users", "Accounts", "Id")
                        .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumn("Address").AsString().NotNullable().Unique()
                .WithColumn("Primary").AsBoolean().NotNullable()
            ;

            // One option here would be to add a composite unique key constraint on AccountId and Primary,
            // but as we want the user to be able to have multiple secondary mail adresses, that is not an option.
            // The constraint should be implemented in the core instead.

            // -SA 2016-07-27

            Create.Index("IX_Emails_AccountId")
                .OnTable("Emails").InSchema("Users").OnColumn("AccountId");
            ;
        }

        public override void Down()
        {
            Delete.Index("IX_Emails_AccountId")
                .OnTable("Emails").InSchema("Users");
            Delete.Table("Emails").InSchema("Users");
        }
    }
}
