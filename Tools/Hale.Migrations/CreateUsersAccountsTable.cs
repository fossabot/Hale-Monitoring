using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Migrations
{
    [Migration(2)]
    public class CreateUsersAccountsTable : Migration
    {

        public override void Up()
        {
            Create.Table("Accounts").InSchema("Users")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Fullname").AsString().Nullable()
                .WithColumn("Username").AsString().NotNullable().Unique()
                .WithColumn("Password").AsFixedLengthString(60).NotNullable()
                .WithColumn("OldPassword").AsFixedLengthString(60).NotNullable()
                .WithColumn("PasswordChanged").AsDateTime().Nullable()
                .WithColumn("Activated").AsBoolean().WithDefaultValue(false)
                .WithColumn("Enabled").AsBoolean().WithDefaultValue(true)
                .WithColumn("Created").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("CreatedBy").AsInt32().NotNullable()
                    .ForeignKey("FK_Accounts_Id_Accounts_CreatedBy", "Users", "Accounts", "Id")
                        .OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumn("Changed").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("ChangedBy").AsInt32().NotNullable()
                    .ForeignKey("FK_Accounts_Id_Accounts_ChangedBy", "Users", "Accounts", "Id")
                        .OnDeleteOrUpdate(System.Data.Rule.Cascade)

                ;
            // "IX_Users_UsernameFullnameId"
            Create.Index("IX_Accounts_Username_Fullname_Id").OnTable("Accounts").InSchema("Users")
                .OnColumn("Username").Ascending()
                .OnColumn("Fullname").Ascending()
                .OnColumn("Id").Ascending()
                .WithOptions().Unique()
                .WithOptions().NonClustered()
                
                ;

        }

        public override void Down()
        {
            Delete.Index("IX_Accounts_Username_Fullname_Id").OnTable("Accounts").InSchema("Users");

            // Should probably be here later. -NM 2016-07-26
            // Delete.ForeignKey("FK_Users_ChangedBy_Users_Id");
            // Delete.ForeignKey("FK_Users_CreatedBy_Users_Id");

            Delete.Table("Accounts").InSchema("Users");
            
        }
    }
}
