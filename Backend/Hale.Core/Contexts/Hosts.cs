using Hale.Core.Entities.Nodes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Hale.Core.Handlers;
using System.Data;

namespace Hale.Core.Contexts
{
    internal class Hosts : SqlHandler
    {
        private readonly int _idNotSet = 0;

        public enum UpdateMode
        {
            Status,
            RsaKey,
            Info
        }

        public void Create (Host host)
        {
            ConnectToDatabase();
            connection.Execute(
                @"INSERT INTO ""Nodes"".""Hosts"" (" +
                @" ""FriendlyName"", ""HostName"", ""Ip""," +
                @") VALUES (" +
                @"@friendlyname, @hostname, @ip" +
                @")",
                new
                {
                    friendlyname = host.FriendlyName,
                    hostname = host.HostName,
                    ip = host.Ip
                }
            );
        }

        // FIXME[SA]: Is there a better way to do this which lets us stick to only the Update method?
        // Overloading with bool does not cut it as we have two different overloads and do not want to have two different bools to set.
        public void Update(Host host, UpdateMode mode = UpdateMode.Info)
        {
            ConnectToDatabase();

            switch (mode)
            {
                case UpdateMode.Info:
                    connection.Execute(@"UPDATE ""Nodes"".""Hosts"" SET "
                    + @"FriendlyName = '@friendlyname'"
                    + @", ""Hostname"" =  '@hostname'"
                    + @", ""Ip"" = '@ip'"
                    + @", ""Guid"" = '@guid'"
                    + @" WHERE ""Id"" = @id",
                    new
                    {
                        friendlyname = host.FriendlyName,
                        hostname = host.HostName,
                        ip = host.Ip,
                        guid = host.Guid,
                        id = host.Id,
                    });
                    break;
                case UpdateMode.Status:
                    connection.Execute(@"UPDATE ""Nodes"".""Hosts"" SET "
                    + @", ""Status"" = '@status'"
                    + @" WHERE ""Id"" = @id",
                    new
                    {
                        id = host.Id,
                        status = host.Status
                    });
                    break;
                case UpdateMode.RsaKey:
                    connection.Execute(@"UPDATE ""Nodes"".""Hosts"" SET ""RsaKey"" = @rsa WHERE ""Id"" =  @id",
                    new
                    {
                        id = host.Id
                        ,
                        rsa = host.RsaKey
                    });
                    break;
            }
        }

        // Part of the fixme above.
        public void UpdateRsa(Host host)
        {
            Update(host, UpdateMode.RsaKey);
        }

        // Part of the fixme above.
        public void UpdateStatus(Host host)
        {
            Update(host, UpdateMode.Status);
        }

        public void Delete (Host host)
        {
            ConnectToDatabase();
            connection.Execute(@"DELETE FROM ""Nodes"".""Hosts"" WHERE ""Id"" = @id",
                new
                {
                    id = host.Id
                }
            );
            
        }

        public Host Get (Host host)
        {
            ConnectToDatabase();
            if (host.Id != _idNotSet)
            {
                return connection.Query<Host>(@"SELECT * FROM ""Nodes"".""Hosts"" WHERE ""Id"" = @id",
                    new
                    {
                        id = host.Id
                    }
                ).First();
            }
            else if (host.Guid != Guid.Empty)
            {
                return connection.Query<Host>(@"SELECT * FROM ""Nodes"".""Hosts"" WHERE ""Guid"" = @guid",
                    new
                    {
                        guid = host.Guid
                    }
                ).First();
            }
            else
            {
                throw new Exception("No suitable selectors found in Host search object.");
            }
        }

        public List<Host> List()
        {
            ConnectToDatabase();
            return connection.Query<Host>(@"SELECT * FROM ""Nodes"".""Hosts"" ").ToList();
        }
    }
}
