namespace Skeleton.Dapper.Tests.Extensions
{
    using System;
    using System.Dynamic;
    using System.Linq;
    using Common.Extensions;
    using global::Dapper;
    using JetBrains.Annotations;
    using Tests;
    using Xunit;
    using Dapper.Extensions;
    using Dapper.Extensions.PropertyInfoProviders;

    public class SqlConnectionExtensionsTests : DbUsingTestBase
    {
        #region TestCases

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

        public class Totals
        {
            public int Id { get; [UsedImplicitly] set; }

            public int First { get; set; }

            public int Second { get; set; }

            public int Sum { get; set; }

            protected bool Equals(Totals other)
            {
                return First == other.First && Second == other.Second && Sum == other.Sum;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Totals) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = First;
                    hashCode = (hashCode * 397) ^ Second;
                    hashCode = (hashCode * 397) ^ Sum;
                    return hashCode;
                }
            }
        }

        public class BulkInsertTestCase<TSource>
        {
            public TSource[] Source { get; set; }
        }

        #endregion

        [UsedImplicitly]
        public static TheoryData<BulkInsertTestCase<TestEntity>> BulkInsertUsageTestsData;
        [UsedImplicitly]
        public static TheoryData<BulkInsertTestCase<Totals>> ManualConfiguredBulkInsertUsageTestsData;

        static SqlConnectionExtensionsTests()
        {
            BulkInsertUsageTestsData =
                new TheoryData<BulkInsertTestCase<TestEntity>>
                {
                    new BulkInsertTestCase<TestEntity>
                    {
                        Source = new[]
                                 {
                                     new TestEntity {Name = "First", Value = 1},
                                     new TestEntity {Name = "Second", Value = 2},
                                     new TestEntity {Name = "Third", Value = 3}
                                 }
                    },
                    new BulkInsertTestCase<TestEntity> {Source = new TestEntity[0]}
                };
            ManualConfiguredBulkInsertUsageTestsData =
                new TheoryData<BulkInsertTestCase<Totals>>
                {
                    new BulkInsertTestCase<Totals>
                    {
                        Source = new[]
                        {
                            new Totals {First = 1, Second = 2, Sum = 3},
                            new Totals {First = -1, Second = 1, Sum = 0},
                            new Totals {First = 100, Second = 12, Sum = 112}
                        }
                    },
                    new BulkInsertTestCase<Totals> {Source = new Totals[0]}
                };
        }

        [Theory]
        [MemberData(nameof(BulkInsertUsageTestsData))]
        public void ShouldPerformBulkInsertWithPropertyInfoProvider(BulkInsertTestCase<TestEntity> testCase)
        {
            // When
            TestEntity[] actual;
            using (var connection = SqlConnectionsFactoryMethod())
            {
                connection.Execute(@"create table #TestEntities (Id int identity(1, 1) not null, Name nvarchar(max) not null, Value int not null)");
                connection.BulkInsert("#TestEntities", testCase.Source, x => new StrictTypeMappingInfoProvider(typeof(TestEntity), x));
                actual = connection.Query<TestEntity>("select * from #TestEntities").ToArray();
            }

            // Then
            Assert.Equal(testCase.Source, actual);
        }

        [Theory]
        [MemberData(nameof(BulkInsertUsageTestsData))]
        public void ShouldPerformBulkInsertInExternalTransaction(BulkInsertTestCase<TestEntity> testCase)
        {
            // When
            TestEntity[] actual;
            using (var connection = SqlConnectionsFactoryMethod())
            {
                connection.Execute(@"create table #TestEntities (Id int identity(1, 1) not null, Name nvarchar(max) not null, Value int not null)");

                using (var transaction = connection.BeginTransaction())
                {
                    connection.BulkInsert("#TestEntities", testCase.Source, x => new StrictTypeMappingInfoProvider(typeof(TestEntity), x), transaction);
                    transaction.Commit();
                }

                actual = connection.Query<TestEntity>("select * from #TestEntities").ToArray();
            }

            // Then
            Assert.Equal(testCase.Source, actual);
        }

        [Theory]
        [MemberData(nameof(BulkInsertUsageTestsData))]
        public void ShouldPerformBulkInsert(BulkInsertTestCase<TestEntity> testCase)
        {
            // When
            TestEntity[] actual;
            using (var connection = SqlConnectionsFactoryMethod())
            {
                connection.Execute(@"create table #TestEntities (Id int identity(1, 1) not null, Name nvarchar(max) not null, Value int not null)");
                connection.BulkInsert("#TestEntities", testCase.Source);
                actual = connection.Query<TestEntity>("select * from #TestEntities").ToArray();
            }

            // Then
            Assert.Equal(testCase.Source, actual);
        }

        [Theory]
        [MemberData(nameof(ManualConfiguredBulkInsertUsageTestsData))]
        public void ShouldPerformBulkInsertWithManuallyConfiguredMapping(BulkInsertTestCase<Totals> testCase)
        {
            // When
            Totals[] actual;
            using (var connection = SqlConnectionsFactoryMethod())
            {
                connection.Execute(@"create table #TestEntities (Id int identity(1, 1) not null, First int not null, Second int not null, Sum int not null)");
                connection.BulkInsert(
                    "#TestEntities",
                    testCase.Source,
                    x => x.WithProperty(entity => entity.First)
                        .WithProperty(entity => entity.Second)
                        .WithFunction(entity => entity.First + entity.Second, "Sum")
                );
                actual = connection.Query<Totals>("select * from #TestEntities").ToArray();
            }

            // Then
            Assert.True(testCase.Source.IsSame(actual));
        }

        [Fact]
        public void SimpleTypePropertyInfoProviderFailTest()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    using (var connection = SqlConnectionsFactoryMethod())
                        connection.BulkInsert(
                            "#TestEntities", new object[] {2},
                            x => new StrictTypeMappingInfoProvider(typeof(object), x)
                        );
                });
        }

        [Fact]
        public void ShouldNotPerformAnyActionIfSourceIsEmpty()
        {
            using (var connection = SqlConnectionsFactoryMethod())
                connection.BulkInsert(
                    "#TestEntities", new ExpandoObject[0],
                    x => new ExpandoObjectMappingInfoProvider(new ExpandoObject(), x)
                );
        }

        [Fact]
        public void ShouldPerformObjectExpandoBulkInsert()
        {
            dynamic item1 = new ExpandoObject();
            item1.Id = 1;
            item1.Name = "First";
            item1.Value = 5;

            dynamic item2 = new ExpandoObject();
            item2.Id = 2;
            item2.Name = "Second";
            item2.Value = 10;

            var expected = new[] { item1, item2 };

            // When
            dynamic[] actual;
            using (var connection = SqlConnectionsFactoryMethod())
            {
                connection.Execute(@"create table #TestEntities (Id int identity(1, 1) not null, Name nvarchar(max) not null, Value int not null)");
                connection.BulkInsert("#TestEntities", expected, x => new ExpandoObjectMappingInfoProvider(item1, x));
                actual = connection.Query("select * from #TestEntities").ToArray();
            }
            // Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldPerformDynamicBulkInsert()
        {
            dynamic expected = new { Id = 1, Name = "First", Value = 5 };

            // When
            dynamic actual;
            using (var connection = SqlConnectionsFactoryMethod())
            {
                connection.Execute(@"create table #TestEntities (Id int identity(1, 1) not null, Name nvarchar(max) not null, Value int not null)");
                connection.BulkInsert("#TestEntities", new [] { expected });
                actual = connection.Query("select * from #TestEntities").Single();
            }
            // Then
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Value, actual.Value);
        }
    }
}