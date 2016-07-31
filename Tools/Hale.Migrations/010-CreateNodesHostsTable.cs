using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Migrations
{
    [Migration(10)]
    public class M010CreateNodesHostsTable: Migration
    {
        public override void Up()
        {
            Create.Table("Hosts").InSchema("Nodes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("FriendlyName").AsString().Nullable()
                .WithColumn("HostName").AsString().Nullable()
                .WithColumn("Ip").AsString().Nullable()
                .WithCreateModify()
                .WithColumn("LastConnection").AsDateTime().Nullable()
                .WithColumn("Guid").AsGuid().NotNullable().WithDefault(SystemMethods.NewGuid)
                .WithColumn("RsaKey").AsBinary(1024).Nullable()
            ;
        }

        public override void Down()
        {
            Delete.Table("Hosts").InSchema("Nodes");
        }
    }
}
