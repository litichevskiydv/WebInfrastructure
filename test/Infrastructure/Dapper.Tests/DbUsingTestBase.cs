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
        private readonly bool _isAppVeyorWindows;
        private readonly bool _isAppVeyorLinux;
        private readonly bool _isTravis;
        private readonly bool _isGithubActions;

        private string ConnectionString
        {
            get
            {
                if (_isAppVeyorWindows)
                    return @"Data Source = (local)\SQL2017;Initial Catalog=tempdb;User Id=sa;Password=Password12!";
                if (_isAppVeyorLinux)
                    return "Data Source = localhost;Initial Catalog=tempdb;User Id=sa;Password=Password12!";
                if (_isGithubActions)
                    return "Data Source = localhost;Initial Catalog=tempdb;User Id=sa;Password=Password12!";
                if (_isTravis)
                    return "Data Source = localhost;Initial Catalog=tempdb;User Id=sa;Password=Password12!";

                return @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = tempdb; Integrated Security = True";
            }
        }

        protected readonly IConnectionsFactory ConnectionsFactory;
        protected readonly Func<SqlConnection> SqlConnectionsFactoryMethod;

        protected DbUsingTestBase()
        {
            var isAppVeyor = Environment.GetEnvironmentVariable("APPVEYOR")?.ToUpperInvariant() == "TRUE";
            _isAppVeyorWindows = isAppVeyor && Environment.GetEnvironmentVariable("CI_WINDOWS")?.ToUpperInvariant() == "TRUE";
            _isAppVeyorLinux = isAppVeyor && Environment.GetEnvironmentVariable("CI_LINUX")?.ToUpperInvariant() == "TRUE";

            _isTravis = Environment.GetEnvironmentVariable("TRAVIS")?.ToUpperInvariant() == "TRUE";

            _isGithubActions = Environment.GetEnvironmentVariable("GITHUB_ACTIONS")?.ToUpperInvariant() == "TRUE";

            ConnectionsFactory = new SqlConnectionsFactory(Options.Create(new SqlConnectionsFactoryOptions {SqlServer = ConnectionString}));
            SqlConnectionsFactoryMethod = () => (SqlConnection) ConnectionsFactory.Create();
        }
    }
}