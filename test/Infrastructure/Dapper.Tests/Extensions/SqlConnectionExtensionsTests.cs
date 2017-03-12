namespace Skeleton.Dapper.Tests.Extensions
{
    using System;
    using System.Dynamic;
    using System.Collections.Generic;
    using System.Linq;
    using global::Dapper;
    using JetBrains.Annotations;
    using Tests;
    using Xunit;
    using Dapper.Extensions;
    using Dapper.Extensions.PropertyInfoProviders;

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
                return obj.GetType() == GetType() && Equals((TestEntity)obj);
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
        public void ShouldPerformBulkInsertWithPropertyInfoProvider(TestEntity[] expected)
        {
            // When
            TestEntity[] actual;
            using (var connection = SqlConnectionsFactoryMethod())
            {
                connection.Execute(@"create table #TestEntities (Id int identity(1, 1) not null, Name nvarchar(max) not null, Value int not null)");
                connection.BulkInsert("#TestEntities", expected, new StrictTypePropertyInfoProvider(typeof(TestEntity)));
                actual = connection.Query<TestEntity>("select * from #TestEntities").ToArray();
            }

            // Then
            Assert.Equal(expected, actual);
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



        [Fact]
        public void StrictTypePropertyInfoProviderTest()
        {
            var expected = new TestEntity { Name = "First", Value = 1, Id = 1 };

            IPropertyInfoProvider provider = new StrictTypePropertyInfoProvider(typeof(TestEntity));

            Assert.Equal(3, provider.FieldCount);
            Assert.Equal("Name", provider.GetName(1));
            Assert.Equal(typeof(int), provider.GetFieldType(2));
            Assert.Equal(0, provider.GetOrdinal("Id"));
            Assert.Equal("First", provider.GetValue(1, expected));
        }

        [Fact]
        public void SimpleTypePropertyInfoProviderFailTest()
        {
            Assert.Throws<InvalidOperationException>(() => new StrictTypePropertyInfoProvider(typeof(object)));
        }

        [Fact]
        public void ExpandoObjectPropertyInfoProviderTest()
        {
            dynamic expected = new ExpandoObject();
            expected.Id = 1;
            expected.Name = "First";
            expected.Value = 5;

            IPropertyInfoProvider provider = new ExpandoObjectPropertyInfoProvider(expected);

            Assert.Equal(3, provider.FieldCount);
            Assert.Equal("Name", provider.GetName(1));
            Assert.Equal(typeof(int), provider.GetFieldType(2));
            Assert.Equal(0, provider.GetOrdinal("Id"));
            Assert.Equal("First", provider.GetValue(1, expected));
        }

        [Fact]
        public void NullExpandoObjectPropertyInfoProviderFailTest()
        {
            Assert.Throws<InvalidOperationException>(() => new ExpandoObjectPropertyInfoProvider(new ExpandoObject()));
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

            var expected = new List<ExpandoObject>() { item1, item2 };

            // When
            dynamic[] actual;
            using (var connection = SqlConnectionsFactoryMethod())
            {
                connection.Execute(@"create table #TestEntities (Id int identity(1, 1) not null, Name nvarchar(max) not null, Value int not null)");
                connection.BulkInsert("#TestEntities", expected, new ExpandoObjectPropertyInfoProvider(item1));
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