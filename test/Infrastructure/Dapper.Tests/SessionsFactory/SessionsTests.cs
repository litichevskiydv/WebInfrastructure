namespace Skeleton.Dapper.Tests.SessionsFactory
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper.Extensions;
    using Dapper.SessionsFactory;
    using Xunit;

    public class SessionsTests : DbUsingTestBase
    {
        private readonly SessionsFactory _sessionsFactory;

        public SessionsTests()
        {
            _sessionsFactory = new SessionsFactory(ConnectionsFactory);
        }

        [Fact]
        public void ShouldGetExceptionOnAlreadyDisposedSession()
        {
            // Given
            var session = _sessionsFactory.Create();

            // When
            session.Dispose();

            // Then
            Assert.Throws<ObjectDisposedException>(() => session.Commit());
        }

        [Fact]
        public void ShouldReadDataFromCommitedTransaction()
        {
            // Given
            var createTableQuery = new QueryObject("create table ##TransactionsTest ([ID] int, [Value] varchar(32));");
            var insertQuery = new QueryObject("insert into ##TransactionsTest ([ID], [Value]) values (1, '123');");
            var countQuery = new QueryObject("select count(*) from ##TransactionsTest;");
            var dropTableQuery = new QueryObject("drop table ##TransactionsTest;");

            using (var connection = ConnectionsFactory.Create())
                try
                {
                    // When
                    connection.Execute(createTableQuery);
                    using (var session = _sessionsFactory.Create())
                    {
                        session.Execute(insertQuery);
                        session.Commit();
                    }

                    // Then
                    int actualRecordsCount;
                    using (var session = _sessionsFactory.Create())
                        actualRecordsCount = session.Query<int>(countQuery).Single();
                    Assert.Equal(1, actualRecordsCount);
                }
                finally
                {
                    connection.Execute(dropTableQuery);
                }
        }

        [Fact]
        public async Task ShouldReadNoDataFromNotCommitedTransaction()
        {
            // Given
            var createTableQuery = new QueryObject("create table ##TransactionsTest ([ID] int, [Value] varchar(32));");
            var insertQuery = new QueryObject("insert into ##TransactionsTest ([ID], [Value]) values (1, '123');");
            var countQuery = new QueryObject("select count(*) from ##TransactionsTest;");
            var dropTableQuery = new QueryObject("drop table ##TransactionsTest;");

            using (var connection = ConnectionsFactory.Create())
                try
                {
                    // When
                    await connection.ExecuteAsync(createTableQuery);
                    using (var session = _sessionsFactory.Create())
                        await session.ExecuteAsync(insertQuery);

                    // Then
                    int actualRecordsCount;
                    using (var session = _sessionsFactory.Create())
                        actualRecordsCount = (await session.QueryAsync<int>(countQuery)).Single();
                    Assert.Equal(0, actualRecordsCount);
                }
                finally
                {
                    await connection.ExecuteAsync(dropTableQuery);
                }
        }
    }
}