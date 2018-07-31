namespace Skeleton.Dapper.Tests.SessionsFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper.Extensions;
    using Dapper.SessionsFactory;
    using JetBrains.Annotations;
    using Xunit;

    public class SessionsTests : DbUsingTestBase
    {
        public class TestEntity
        {
            public int Id { get; [UsedImplicitly]set; }

            public string Name { get; set; }

            public int Value { get; set; }


            protected bool Equals(TestEntity other)
            {
                return string.Equals(Name, other.Name) && Value == other.Value;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj.GetType() == GetType() && Equals((TestEntity)obj);
            }
        }

        private readonly SessionsFactory _sessionsFactory;

        [UsedImplicitly]
        public static IEnumerable<object[]> BulkInsertUsageTestsData;

        static SessionsTests()
        {
            BulkInsertUsageTestsData =
                new[]
                {
                    new object[]
                    {
                        new[]
                        {
                            new TestEntity {Name = "First", Value = 1},
                            new TestEntity {Name = "Second", Value = 2},
                            new TestEntity {Name = "Third", Value = 3}
                        }
                    },
                    new object[] {new TestEntity[0]}
                };
        }

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

        [Theory]
        [MemberData(nameof(BulkInsertUsageTestsData))]
        public void ShouldPerformBulkInsert(TestEntity[] expected)
        {
            // Given
            var createTableQuery = new QueryObject("create table ##TransactionsTest (Id int identity(1, 1) not null, Name nvarchar(max) not null, Value int not null);");
            var selectAllQuery = new QueryObject("select * from ##TransactionsTest");
            var dropTableQuery = new QueryObject("drop table ##TransactionsTest;");

            // When
            TestEntity[] actual;
            using (var connection = ConnectionsFactory.Create())
                try
                {
                    connection.Execute(createTableQuery);

                    using (var session = _sessionsFactory.Create())
                    {
                        session.BulkInsert("##TransactionsTest", expected);
                        session.Commit();
                    }

                    actual = connection.Query<TestEntity>(selectAllQuery).ToArray();
                }
                finally
                {
                    connection.Execute(dropTableQuery);
                }

            // Then
            Assert.Equal(expected, actual);
        }
    }
}