namespace Skeleton.Dapper.Tests.Tvp
{
    using Common.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dapper.Tvp;
    using JetBrains.Annotations;
    using Xunit;
    using global::Dapper;

    public class TvpParameterTests : DbUsingTestBase
    {
        public class TestEntityWithAllTypes
        {
            public int Id { get; set; }
            public byte? Value1 { get; set; }
            public short? Value2 { get; set; }
            public long? Value3 { get; set; }
            public float? Value4 { get; set; }
            public double? Value5 { get; set; }
            public decimal? Value6 { get; set; }
            public bool? Value7 { get; set; }
            public Guid? Value8 { get; set; }
            public DateTime? Value9 { get; set; }
            public DateTimeOffset? Value10 { get; set; }
            public byte[] Value12 { get; set; }
            public string Value14 { get; set; }
            public char? Value15 { get; set; }

            protected bool Equals(TestEntityWithAllTypes other)
            {
                return
                    Id == other.Id
                    && Value1.Equals(other.Value1)
                    && Value2.Equals(other.Value2)
                    && Value3.Equals(other.Value3)
                    && Value4.Equals(other.Value4)
                    && Value5.Equals(other.Value5)
                    && Value6.Equals(other.Value6)
                    && Value7.Equals(other.Value7)
                    && Value8.Equals(other.Value8)
                    && Value9.Equals(other.Value9)
                    && Value10.Equals(other.Value10)
                    && Value12.IsEquals(other.Value12)
                    && string.Equals(Value14, other.Value14)
                    && Value15.Equals(other.Value15);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((TestEntityWithAllTypes)obj);
            }
        }

        [UsedImplicitly]
        public static readonly IEnumerable<object[]> TvpParameterUsageTestsData;

        static TvpParameterTests()
        {
            TvpParameterUsageTestsData = new[]
                                         {
                                             new object[]
                                             {
                                                 new[]
                                                 {
                                                     new TestEntityWithAllTypes
                                                     {
                                                         Id = 1,
                                                         Value1 = 1,
                                                         Value2 = 2,
                                                         Value3 = 12L,
                                                         Value4 = 1.5f,
                                                         Value5 = 1.7d,
                                                         Value6 = 1.834m,
                                                         Value7 = true,
                                                         Value8 = Guid.NewGuid(),
                                                         Value9 = new DateTime(2016, 01, 01, 00, 00, 00, DateTimeKind.Unspecified),
                                                         Value10 = DateTimeOffset.Now,
                                                         Value12 = new byte[] {5, 6, 7},
                                                         Value14 = "abcde",
                                                         Value15 = 'f'
                                                     },
                                                     new TestEntityWithAllTypes
                                                     {
                                                         Id = 1,
                                                         Value1 = 1,
                                                         Value2 = 2,
                                                         Value4 = 1.5f,
                                                         Value5 = 1.7d,
                                                         Value10 = DateTimeOffset.Now,
                                                         Value12 = new byte[0],
                                                         Value14 = "ab",
                                                         Value15 = 'l'
                                                     }
                                                 }
                                             },
                                             new object[] {new TestEntityWithAllTypes[0]}
                                         };
        }

        [Theory]
        [MemberData(nameof(TvpParameterUsageTestsData))]
        public void ShouldUseTvpInQueryWithAllTypes(TestEntityWithAllTypes[] expected)
        {
            // When
            TestEntityWithAllTypes[] actual;
            using (var connection = GetConnection())
            {
                connection.Execute(@"
if type_id (N'[dbo].[TestListWithAllTypes]') is null
	create type [dbo].[TestListWithAllTypes] as table(
	    [Id] [int] not null,
        [Value1] [tinyint] null,
        [Value2] [smallint] null,
		[Value3] [bigint] null,
		[Value4] [real] null,
		[Value5] [float] null,
		[Value6] [decimal](6,3) null,
		[Value7] [bit] null,
		[Value8] [uniqueidentifier] null,
		[Value9] [datetime] null,
		[Value10] [datetimeoffset] null,
		[Value12] [varbinary](max) null,
		[Value14] [nvarchar](6) null,
		[Value15] [nchar](1) null)");

                actual = connection.Query<TestEntityWithAllTypes>(@"
select * from @Param",
                        new
                        {
                            Param = TvpParameter.Create("TestListWithAllTypes", expected,
                                configurator => configurator
                                    .SetAccuracy(x => x.Value6, 6, 3)
                                    .SetMaxLength(x => x.Value14, 6))
                        })
                    .ToArray();
            }

            // Then
            Assert.Equal(expected, actual);
        }
    }
}