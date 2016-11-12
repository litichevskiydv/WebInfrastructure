namespace Skeleton.Dapper.Tests.Tvp
{
    using System.Collections.Generic;
    using System.Linq;
    using Dapper.Tvp;
    using JetBrains.Annotations;
    using Xunit;
    using global::Dapper;

    public class PrimitiveValuesListTests : DbUsingTestBase
    {
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> Int32ValuesListTestsData;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> Int64ValuesListTestsData;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> DecimalValuesListTestsData;

        static PrimitiveValuesListTests()
        {
            Int32ValuesListTestsData = new[]
                                       {
                                           new object[] {Enumerable.Range(0, 11).ToArray()},
                                           new object[] {new int[0]}
                                       };
            Int64ValuesListTestsData = new[]
                                       {
                                           new object[] {new[] {1000000000L, 1000000001L, 1000000002L}},
                                           new object[] {new long[0]}
                                       };
            DecimalValuesListTestsData = new[]
                                         {
                                             new object[] {new[] {1.234m, 5.678m, 99.123m}},
                                             new object[] {new decimal[0]}
                                         };
        }

        [Theory]
        [MemberData(nameof(Int32ValuesListTestsData))]
        public void ShouldUseInt32ValuesListInQuery(int[] expected)
        {
            // When
            int[] actual;
            using (var connection = GetConnection())
            {
                connection.Execute(@"
if type_id (N'[dbo].[Int32ValuesList]') is null
	create type [dbo].[Int32ValuesList] as table([Value] [int] not null)");

                actual = connection.Query<int>(@"
select [Value] from @Param",
                        new {Param = new Int32ValuesList("Int32ValuesList", expected)})
                    .ToArray();
            }

            // Then
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(Int64ValuesListTestsData))]
        public void ShouldUseInt64ValuesListInQuery(long[] expected)
        {
            // When
            long[] actual;
            using (var connection = GetConnection())
            {
                connection.Execute(@"
if type_id (N'[dbo].[Int64ValuesList]') is null
	create type [dbo].[Int64ValuesList] as table([Value] [bigint] not null)");

                actual = connection.Query<long>(@"
select [Value] from @Param",
                        new { Param = new Int64ValuesList("Int64ValuesList", expected) })
                    .ToArray();
            }

            // Then
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(DecimalValuesListTestsData))]
        public void ShouldUseDecimalValuesListInQuery(decimal[] expected)
        {
            // When
            decimal[] actual;
            using (var connection = GetConnection())
            {
                connection.Execute(@"
if type_id (N'[dbo].[DecimalValuesList]') is null
	create type [dbo].[DecimalValuesList] as table([Value] [numeric](6,3) not null)");

                actual = connection.Query<decimal>(@"
select [Value] from @Param",
                        new
                        {
                            Param = PrimitiveValuesList.Create("DecimalValuesList", expected,
                                new MetaDataCreationOptions {Precision = 6, Scale = 3})
                        })
                    .ToArray();
            }

            // Then
            Assert.Equal(expected, actual);
        }
    }
}