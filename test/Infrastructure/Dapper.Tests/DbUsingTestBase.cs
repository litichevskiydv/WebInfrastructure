namespace Skeleton.Dapper.Tests
{
    using System;
    using System.Data.SqlClient;
    using ConnectionsFactory;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Options;

    [UsedImplicitly]
    public abstract class DbUsingTestBase
    {
        private readonly bool _isAppVeyor;
        private string ConnectionString =>
            _isAppVeyor
                ? @"Data Source = (local)\SQL2016;Initial Catalog=tempdb;User Id=sa;Password=Password12!"
                : @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = tempdb; Integrated Security = True";

        protected readonly IConnectionsFactory ConnectionsFactory;
        protected readonly Func<SqlConnection> SqlConnectionsFactoryMethod;

        protected DbUsingTestBase()
        {
            _isAppVeyor = Environment.GetEnvironmentVariable("Appveyor")?.ToUpperInvariant() == "TRUE";

            ConnectionsFactory = new SqlConnectionsFactory(Options.Create(new SqlConnectionsFactoryOptions {SqlServer = ConnectionString}));
            SqlConnectionsFactoryMethod = () => (SqlConnection) ConnectionsFactory.Create();
        }
    }
}