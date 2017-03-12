namespace Skeleton.Dapper.ConnectionsFactory
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class SqlConnectionsFactory : IConnectionsFactory
    {
        private readonly string _connectionString;

        public SqlConnectionsFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
        }

        public IDbConnection Create()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}