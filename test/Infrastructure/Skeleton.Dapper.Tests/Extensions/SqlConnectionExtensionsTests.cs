namespace Skeleton.Dapper.Tests.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Dapper.Extensions;
    using global::Dapper;
    using JetBrains.Annotations;
    using Tests;
    using Xunit;

    public class SqlConnectionExtensionsTests : DbUsingTestBase
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
                return obj.GetType() == GetType() && Equals((TestEntity) obj);
            }
        }

        [UsedImplicitly]
        public static IEnumerable<object[]> BulkInsertUsageTestsData;

        static SqlConnectionExtensionsTests()
        {
            BulkInsertUsageTestsData = new[]
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

        [Theory]
        [MemberData(nameof(BulkInsertUsageTestsData))]
        public void ShouldPerformBulkInsert(TestEntity[] expected)
        {
            // When
            TestEntity[] actual;
            using (var connection = SqlConnectionsFactoryMethod())
            {
                connection.Execute(@"create table #TestEntities (Id int identity(1, 1) not null, Name nvarchar(max) not null, Value int not null)");
                connection.BulkInsert("#TestEntities", expected);
                actual = connection.Query<TestEntity>("select * from #TestEntities").ToArray();
            }

            // Then
            Assert.Equal(expected, actual);
        }
    }
}