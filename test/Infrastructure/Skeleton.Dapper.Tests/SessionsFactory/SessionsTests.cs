namespace Skeleton.Dapper.Tests.SessionsFactory
{
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper.SessionsFactory;
    using global::Dapper;
    using Xunit;

    public class SessionsTests : DbUsingTestBase
    {
        private readonly SessionsFactory _sessionsFactory;

        public SessionsTests()
        {
            _sessionsFactory = new SessionsFactory(ConnectionsFactory);
        }

        [Fact]
        public void ShouldReadDataFromCommitedTransaction()
        {
            using (var connection = ConnectionsFactory.Create())
                try
                {
                    // Given
                    connection.Execute("create table ##TransactionsTest ([ID] int, [Value] varchar(32));");

                    // When
                    using (var session = _sessionsFactory.Create())
                    {
                        session.Execute("insert into ##TransactionsTest ([ID], [Value]) values (1, '123');");
                        session.Commit();
                    }

                    // Then
                    int actualRecordsCount;
                    using (var session = _sessionsFactory.Create())
                        actualRecordsCount = session.Query<int>("select count(*) from ##TransactionsTest;").Single();
                    Assert.Equal(1, actualRecordsCount);
                }
                finally
                {
                    connection.Execute("drop table ##TransactionsTest;");
                }
        }

        [Fact]
        public async Task ShouldReadNoDataFromNotCommitedTransaction()
        {
            using (var connection = ConnectionsFactory.Create())
                try
                {
                    // Given
                    connection.Execute("create table ##TransactionsTest ([ID] int, [Value] varchar(32));");

                    // When
                    using (var session = _sessionsFactory.Create())
                        await session.ExecuteAsync("insert into ##TransactionsTest ([ID], [Value]) values (1, '123');");

                    // Then
                    int actualRecordsCount;
                    using (var session = _sessionsFactory.Create())
                        actualRecordsCount = (await session.QueryAsync<int>("select count(*) from ##TransactionsTest;")).Single();
                    Assert.Equal(0, actualRecordsCount);
                }
                finally
                {
                    connection.Execute("drop table ##TransactionsTest;");
                }
        }
    }
}