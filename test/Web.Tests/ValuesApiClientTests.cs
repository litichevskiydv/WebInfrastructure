namespace Web.Tests
{
    using System.Collections.Generic;
    using Client;
    using Skeleton.Web.Testing;
    using Skeleton.Web.Testing.Extensions;
    using Xunit;

    public class ValuesApiClientTests : BaseApiClientTests<BaseApiTestsFixture<TestsStartup>, ApiClient>
    {
        public ValuesApiClientTests(BaseApiTestsFixture<TestsStartup> fixture) : base(fixture)
        {
        }

        [Fact]
        public void ShouldReturnValues()
        {
            // When
            ApiClient
                .GetValues();

            // Then
            Assert.NotEmpty((IEnumerable<string>)ApiClient.CurrentState);
            Fixture.Logger.VerifyNoErrors();
        }

        [Fact]
        public void ShouldSetValue()
        {
            // Given
            const int id = 1;
            const string expectedValue = "test";

            // When
            ApiClient
                .SetValue(id, expectedValue)
                .GetValue(id);

            // Then
            Assert.Equal(expectedValue, (string)ApiClient.CurrentState);
            Fixture.Logger.VerifyNoErrors();
        }
    }
}