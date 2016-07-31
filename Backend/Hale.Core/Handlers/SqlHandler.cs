using Hale.Core.Config;
using NLog;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security;
using System.Text;
using System.Threading;
using Npgsql;


namespace Hale.Core.Handlers
{
    abstract internal class SqlHandler
    {
        private string connectionString;
        private string databaseName;

        private Logger log;
        private Configuration config;

        private readonly int connectionAttempts = 3;
        private readonly int connectionDelay = 3;

        internal IDbConnection connection;

        internal SqlHandler()
        {
            databaseName = "HaleDB";

            LoadLogger();
            LoadConfiguration();
            LoadConnectionString();

        }

        private void LoadLogger()
        {
            log = LogManager.GetCurrentClassLogger();
        }

        private void LoadConfiguration()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // Note: I have replaced the instanced variable with config.Database(), which has to be
            // a method since property extensions does not exist in C#. 
            // Perhaps this should be reverted? -NM 2016-01-17
        }


        private void LoadConnectionString()
        {
            try
            {
                var _db = config.Database();
                if (_db.Type == "mssql")
                {
                    SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
                    connectionStringBuilder.DataSource = config.Database().Host;
                    connectionStringBuilder.InitialCatalog = config.Database().Database;
                    connectionStringBuilder.IntegratedSecurity = config.Database().UseIntegratedSecurity;
                    connectionString = connectionStringBuilder.ToString();
                }
                else if(_db.Type == "postgres")
                {
                    var csb = new NpgsqlConnectionStringBuilder();
                    csb.Host = _db.Host;
                    csb.Username = _db.User;
                    csb.Password = _db.Password;
                    csb.Database = _db.Database;
                    connectionString = csb.ToString();
                }

            }
            catch
            {
                log.Error("Could not setup connection string for \"" + databaseName + "\". Check HaleCore.config");
            }

        }

        private SqlCredential LoadCredentials()
        {
            var ca = config.Database().Password.ToCharArray();
            SecureString pw = new SecureString();
            foreach (char t in ca)
            {
                pw.AppendChar(t);
            }
            pw.MakeReadOnly();
            return new SqlCredential(config.Database().User, pw);

        }

        internal void ConnectToDatabase()
        {
            connection = null;

            var _db = config.Database();
            if (_db.Type == "mssql")
            {
                if (config.Database().UseIntegratedSecurity)
                {
                    connection = new SqlConnection(connectionString);

                }
                else
                {
                    SqlCredential credentials = LoadCredentials();
                    connection = new SqlConnection(connectionString, credentials);
                }
            }
            else if (_db.Type == "postgres")
            {
                connection = new NpgsqlConnection(connectionString);
            }
            else
            {
                throw new NotImplementedException($"Datbase type \"{_db.Type}\" is not implemented.");
            }
            ConnectWithRetries();

        }

        private void ConnectWithRetries()
        {
            for (int i = 1; i < connectionAttempts; i++)
            {
                try
                {
                    connection.Open();
                    break;
                }
                catch (InvalidOperationException e)
                {
                    log.Error("Could not execute the requested operation:" + e.Message);
                }
                catch (SqlException e)
                {
                    if (i < connectionAttempts)
                    {
                        log.Warn(
                            "Connection attempt " + i + " of " + connectionAttempts
                            + " failed. Retrying in " + connectionDelay + " seconds."
                        );
                        Thread.Sleep(TimeSpan.FromSeconds(connectionDelay));
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }
    }
}
