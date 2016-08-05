using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using Hale.Core.Config;
using NLog;
using Npgsql;
using System.Security;
using System.Data;
using System.Threading;
using DapperExtensions;
using Hale_Core.Config;

namespace Hale_Core.Handlers
{
    class DataProvider : IDataProvider
    {
        private string _databaseName;
        private string _connectionString;
        private Configuration _config;
        private Logger _log;

        private readonly int _connectionAttempts = 3;
        private readonly int _connectionDelay = 3;

        internal IDbConnection connection;

        public DataProvider()
        {
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            _log = LogManager.GetCurrentClassLogger();
            _databaseName = "HaleDB";
        }

        private void LoadConnectionString()
        {
            try
            {
                var _db = _config.Database();

                if (_db.Type.Equals(DatabaseType.SqlServer))
                    BuildMSSQLConnectionString();
                else if (_db.Type.Equals(DatabaseType.PostgreSql))
                    BuildPGSQLConnectionString(_db);

            }
            catch
            {
                _log.Error("Could not setup connection string for \"" + _databaseName + "\". Check HaleCore.config");
            }

        }

        private void BuildPGSQLConnectionString(DatabaseSection _db)
        {
            var csb = new NpgsqlConnectionStringBuilder();
            csb.Host = _db.Host;
            csb.Username = _db.User;
            csb.Password = _db.Password;
            csb.Database = _db.Database;
            _connectionString = csb.ToString();
        }

        private void BuildMSSQLConnectionString()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.DataSource = _config.Database().Host;
            connectionStringBuilder.InitialCatalog = _config.Database().Database;
            connectionStringBuilder.IntegratedSecurity = _config.Database().UseIntegratedSecurity;
            _connectionString = connectionStringBuilder.ToString();
        }

        private SqlCredential LoadCredentials()
        {
            var ca = _config.Database().Password.ToCharArray();
            SecureString pw = new SecureString();

            foreach (char t in ca)
            {
                pw.AppendChar(t);
            }

            pw.MakeReadOnly();
            return new SqlCredential(_config.Database().User, pw);

        }

        internal void ConnectToDatabase()
        {
            connection = null;
            var _db = _config.Database();

            if (_db.Type.Equals(DatabaseType.SqlServer))
                ConnectToMSSQLDatabase();

            else if (_db.Type.Equals(DatabaseType.PostgreSql))
                ConnectToPGSQLDatabase();

            else
                throw new NotImplementedException($"Datbase type \"{_db.Type}\" is not implemented.");

            ConnectWithRetries();

        }

        private void ConnectWithRetries()
        {
            for (int i = 1; i < _connectionAttempts; i++)
            {
                try
                {
                    connection.Open();
                    break;
                }
                catch (InvalidOperationException e)
                {
                    _log.Error("Could not execute the requested operation:" + e.Message);
                }
                catch (SqlException e)
                {
                    if (i < _connectionAttempts)
                    {
                        _log.Warn(
                            "Connection attempt " + i + " of " + _connectionAttempts
                            + " failed. Retrying in " + _connectionDelay + " seconds."
                        );
                        Thread.Sleep(TimeSpan.FromSeconds(_connectionDelay));
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }

        private void ConnectToPGSQLDatabase()
        {
            connection = new NpgsqlConnection(_connectionString);
        }

        private void ConnectToMSSQLDatabase()
        {
            if (_config.Database().UseIntegratedSecurity)
            {
                connection = new SqlConnection(_connectionString);

            }
            else
            {
                SqlCredential credentials = LoadCredentials();
                connection = new SqlConnection(_connectionString, credentials);
            }
        }

        public void Create<T>(T model) where T : class
        {
            ConnectToDatabase();
            connection.Insert<T>(model);
        }

        public T Get<T>(int id) where T : class
        {
            ConnectToDatabase();
            return connection.Get<T>(id);
        }

        public IQueryable<T> List<T>() where T : class
        {
            ConnectToDatabase();
            return connection.GetList<T>().AsQueryable();
        }

        public IQueryable<T> List<T>(IPredicate predicates) where T : class
        {
            ConnectToDatabase();
            return connection.GetList<T>(predicates).AsQueryable();
        }

        public void Update<T>(T model) where T : class
        {
            ConnectToDatabase();
            connection.Update<T>(model);
        }

        public void Delete<T>(T model) where T : class
        {
            ConnectToDatabase();
            connection.Delete<T>(model);
        }

    }
}
