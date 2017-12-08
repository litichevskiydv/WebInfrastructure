namespace Skeleton.Dapper.Tests.TypesHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Extensions;
    using Dapper.TypesHandlers;
    using global::Dapper;
    using JetBrains.Annotations;
    using Xunit;

    public class CollectionsHandlerTests : DbUsingTestBase
    {
        public class TestEntity
        {
            public int Id { get; set; }

            public int[] Values { get; set; }

            protected bool Equals(TestEntity other)
            {
                return Values.IsSame(other.Values);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((TestEntity) obj);
            }
        }

        public class WrongEntity
        {
            public int Value { get; set; }
        }

        [UsedImplicitly]
        public static IEnumerable<object[]> CollectionsHandlerUsageTestsData;

        static CollectionsHandlerTests()
        {
            CollectionsHandlerUsageTestsData =
                new[]
                {
                    new object[]
                    {
                        false,
                        new TestEntity {Values = new[] {1, 2, 3}},
                        new TestEntity {Values = new[] {1, 2, 3}}
                    },
                    new object[]
                    {
                        false,
                        new TestEntity {Values = new int[0]},
                        new TestEntity {Values = new int[0]}
                    },
                    new object[]
                    {
                        false,
                        new TestEntity {Values = null},
                        new TestEntity {Values = null}
                    },
                    new object[]
                    {
                        true,
                        new TestEntity {Values = new[] {1, 2, 3}},
                        new TestEntity {Values = new[] {1, 2, 3}}
                    },
                    new object[]
                    {
                        true,
                        new TestEntity {Values = new int[0]},
                        new TestEntity {Values = null}
                    },
                    new object[]
                    {
                        true,
                        new TestEntity {Values = null},
                        new TestEntity {Values = null}
                    }
                };
        }

        [Theory]
        [MemberData(nameof(CollectionsHandlerUsageTestsData))]
        public void ShouldCollectionsHandler(
            bool handleEmptyCollectionAsNull,
            TestEntity source,
            TestEntity expected)
        {
            // Given
            SqlMapper.AddTypeHandler(typeof(int[]), new CollectionsHandler<int>(handleEmptyCollectionAsNull));

            // When
            TestEntity actual;
            using (var connection = SqlConnectionsFactoryMethod())
            {
                connection.Execute(@"create table #TestEntities (Id int identity(1, 1) not null, [Values] nvarchar(max) null)");
                connection.Execute("insert into #TestEntities ([Values]) values(@Values)", source);
                actual = connection.Query<TestEntity>("select * from #TestEntities").Single();
            }

            // Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldThrowExceptionIfNotACollectionIsWritten()
        {
            // Given
            SqlMapper.AddTypeHandler(typeof(WrongEntity), new CollectionsHandler<int>(false));

            // When, Then
            using (var connection = SqlConnectionsFactoryMethod())
            {
                connection.Execute(@"create table #TestEntities (Id int identity(1, 1) not null, [Value] nvarchar(max) null)");
                Assert.Throws<ArgumentException>(
                    () => connection.Execute("insert into #TestEntities ([Value]) values(@Value)",
                        new {Value = new WrongEntity {Value = 1}})
                );
            }
        }

        [Fact]
        public void ShouldThrowExceptionIfNotACollectionIsRead()
        {
            // Given
            SqlMapper.AddTypeHandler(typeof(WrongEntity), new CollectionsHandler<int>(false));

            // When, Then
            using (var connection = SqlConnectionsFactoryMethod())
            {
                connection.Execute(@"
create table #TestEntities (Id int identity(1, 1) not null, [Value] nvarchar(max) null)
insert into #TestEntities ([Value]) values('[1, 2,3]')");
                Assert.Throws<ArgumentException>(
                    () => connection.Query<WrongEntity>("select * from #TestEntities").ToArray()
                );
            }
        }
    }
}