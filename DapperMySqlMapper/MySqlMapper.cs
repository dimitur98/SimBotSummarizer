using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;

namespace DapperMySqlMapper
{
    public class MySqlMapper
    {
        public string ConnectionString { get; set; }
        public int? UnableToConnectToHostErrorRetryInterval { get; private set; }
        public bool UnableToConnectToHostErrorRetryTillConnect { get; private set; }

        public MySqlMapper(string connectionString, int? unableToConnectToHostErrorRetryInterval = null, bool unableToConnectToHostErrorRetryTillConnect = false)
        {
            this.ConnectionString = connectionString;
            this.UnableToConnectToHostErrorRetryInterval = unableToConnectToHostErrorRetryInterval;
            this.UnableToConnectToHostErrorRetryTillConnect = unableToConnectToHostErrorRetryTillConnect;
        }

        public MySqlConnection GetConnection(bool openConnection = true, bool retryOnFailure = true)
        {
            var conn = new MySqlConnection(this.ConnectionString);

            if (openConnection)
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException ex)
                {
                    int? retryInterval = this.UnableToConnectToHostErrorRetryInterval;
                    bool retryTillConnect = this.UnableToConnectToHostErrorRetryTillConnect;

                    if (retryTillConnect == true && retryInterval.HasValue == false) { retryInterval = 5000; }

                    if (ex.Number == (int)MySqlErrorCode.UnableToConnectToHost
                        && retryOnFailure == true
                        && retryInterval.HasValue
                        && retryInterval.Value > 0)
                    {
                        Thread.Sleep(retryInterval.Value);

                        return this.GetConnection(openConnection, retryTillConnect);
                    }

                    throw;
                }
                catch
                {
                    throw;
                }
            }

            return conn;
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = this.GetConnection())
            {
                return this.Query(connection, sql, param, transaction, buffered, commandTimeout, commandType);
            }
        }

        public IEnumerable<dynamic> Query(IDbConnection connection, string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.Query(connection, sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = this.GetConnection())
            {
                return this.Query<T>(connection, sql, param, transaction, buffered, commandTimeout, commandType);
            }
        }

        public void SetTypeMap<T>()
        {
            SqlMapper.SetTypeMap(typeof(T), new CustomPropertyTypeMap(typeof(T), (type, columnName) =>
                                type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .FirstOrDefault(prop => prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))
                                )
                       );
        }

        public IEnumerable<T> Query<T>(IDbConnection connection, string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            this.SetTypeMap<T>();

            return SqlMapper.Query<T>(connection, sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public int Execute(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = this.GetConnection())
            {
                return this.Execute(connection, sql, param, transaction, commandTimeout, commandType);
            }
        }

        public int Execute(IDbConnection connection, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.Execute(connection, sql, param, transaction, commandTimeout, commandType);
        }
    }
}
