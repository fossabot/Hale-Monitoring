using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Migrations
{
    public partial class DevelopmentProfile
    {
        public void SeedHosts()
        {
            Insert.IntoTable("Hosts").InSchema("Nodes")
                .Row(new
                {
                    HostName = "localhost",
                    Ip = "127.0.0.1",
                    Guid = "5b663393-c8ab-4492-b499-9b2a71b71f74"
                })
            ;
        }
    }
}


/*
 
     */
