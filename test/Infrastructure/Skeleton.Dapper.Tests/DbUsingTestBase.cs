namespace Skeleton.Dapper.Tests
{
    using System;
    using System.Data.SqlClient;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public abstract class DbUsingTestBase
    {
        private static readonly bool IsAppVeyor = Environment.GetEnvironmentVariable("Appveyor")?.ToUpperInvariant() == "TRUE";

        protected static string ConnectionString =>
            IsAppVeyor
                ? @"Data Source = (local)\SQL2014;Initial Catalog=tempdb;User Id=sa;Password=Password12!"
                : @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = tempdb; Integrated Security = True";

        protected static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}