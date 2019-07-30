namespace Skeleton.Dapper.Tests.Tvp
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper.Extensions;
    using Dapper.Tvp;
    using JetBrains.Annotations;
    using Xunit;
    using global::Dapper;

    public class PrimitiveValuesListTests : DbUsingTestBase
    {
        #region TestCases

        public class ValuesListUsageTestCase<TSource>
        {
            public TSource[] Source { get; set; }
        }

        #endregion

        [UsedImplicitly]
        public static readonly TheoryData<ValuesListUsageTestCase<int>> Int32ValuesListTestsData;
        [UsedImplicitly]
        public static readonly TheoryData<ValuesListUsageTestCase<long>> Int64ValuesListTestsData;
        [UsedImplicitly]
        public static readonly TheoryData<ValuesListUsageTestCase<decimal>> DecimalValuesListTestsData;

        static PrimitiveValuesListTests()
        {
            Int32ValuesListTestsData =
                new TheoryData<ValuesListUsageTestCase<int>>
                {
                    new ValuesListUsageTestCase<int> {Source = Enumerable.Range(0, 11).ToArray()},
                    new ValuesListUsageTestCase<int> {Source = new int[0]}
                };
            Int64ValuesListTestsData =
                new TheoryData<ValuesListUsageTestCase<long>>
                {
                    new ValuesListUsageTestCase<long> {Source = new[] {1000000000L, 1000000001L, 1000000002L}},
                    new ValuesListUsageTestCase<long> {Source = new long[0]}
                };
            DecimalValuesListTestsData =
                new TheoryData<ValuesListUsageTestCase<decimal>>
                {
                    new ValuesListUsageTestCase<decimal> {Source = new[] {1.234m, 5.678m, 99.123m}},
                    new ValuesListUsageTestCase<decimal> {Source = new decimal[0]}
                };
        }

        private static QueryObject CreateTableQuery()
        {
            return new QueryObject(@"
if type_id (N'[dbo].[Int32ValuesList]') is null
    create type [dbo].[Int32ValuesList] as table([Value] [int] not null)");
        }

        private static QueryObject GetAllValuesQuery(IEnumerable<int> values)
        {
            return new QueryObject("select [Value] from @Param",
                new {Param = new Int32ValuesList("Int32ValuesList", values)});
        }

        [Theory]
        [MemberData(nameof(Int32ValuesListTestsData))]
        public async Task ShouldUseInt32ValuesListInQuery(ValuesListUsageTestCase<int> testCase)
        {
            // When
            int[] actual;
            using (var connection = ConnectionsFactory.Create())
            {
                connection.Execute(CreateTableQuery());
                actual = (await connection.QueryAsync<int>(GetAllValuesQuery(testCase.Source))).ToArray();
            }

            // Then
            Assert.Equal(testCase.Source, actual);
        }

        [Theory]
        [MemberData(nameof(Int64ValuesListTestsData))]
        public void ShouldUseInt64ValuesListInQuery(ValuesListUsageTestCase<long> testCase)
        {
            // When
            long[] actual;
            using (var connection = ConnectionsFactory.Create())
            {
                connection.Execute(@"
if type_id (N'[dbo].[Int64ValuesList]') is null
    create type [dbo].[Int64ValuesList] as table([Value] [bigint] not null)");

                actual = connection.Query<long>(@"
select [Value] from @Param",
                        new { Param = new Int64ValuesList("Int64ValuesList", testCase.Source) })
                    .ToArray();
            }

            // Then
            Assert.Equal(testCase.Source, actual);
        }

        [Theory]
        [MemberData(nameof(DecimalValuesListTestsData))]
        public void ShouldUseDecimalValuesListInQuery(ValuesListUsageTestCase<decimal> testCase)
        {
            // When
            decimal[] actual;
            using (var connection = ConnectionsFactory.Create())
            {
                connection.Execute(@"
if type_id (N'[dbo].[DecimalValuesList]') is null
    create type [dbo].[DecimalValuesList] as table([Value] [numeric](6,3) not null)");

                actual = connection.Query<decimal>(@"
select [Value] from @Param",
                        new
                        {
                            Param = PrimitiveValuesList.Create(
                                "DecimalValuesList",
                                testCase.Source,
                                new MetaDataCreationOptions {Precision = 6, Scale = 3}
                            )
                        })
                    .ToArray();
            }

            // Then
            Assert.Equal(testCase.Source, actual);
        }
    }
}