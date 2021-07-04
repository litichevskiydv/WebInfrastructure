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
        private readonly bool _isGithubActions;

        private string ConnectionString
        {
            get
            {
                if (_isAppVeyor)
                    return @"Data Source = (local)\SQL2017;Initial Catalog=tempdb;User Id=sa;Password=Password12!";
                if (_isGithubActions)
                    return "Data Source = localhost;Initial Catalog=tempdb;User Id=sa;Password=Password12!";

                return @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = tempdb; Integrated Security = True";
            }
        }

        protected readonly IConnectionsFactory ConnectionsFactory;
        protected readonly Func<SqlConnection> SqlConnectionsFactoryMethod;

        protected DbUsingTestBase()
        {
            _isAppVeyor = Environment.GetEnvironmentVariable("APPVEYOR")?.ToUpperInvariant() == "TRUE";
            _isGithubActions = Environment.GetEnvironmentVariable("GITHUB_ACTIONS")?.ToUpperInvariant() == "TRUE";

            ConnectionsFactory = new SqlConnectionsFactory(Options.Create(new SqlConnectionsFactoryOptions {SqlServer = ConnectionString}));
            SqlConnectionsFactoryMethod = () => (SqlConnection) ConnectionsFactory.Create();
        }
    }
}