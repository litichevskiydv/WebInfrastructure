namespace Web.Tests
{
    using System.Collections.Generic;
    using Client;
    using Infrastructure.Web.Testing;
    using Infrastructure.Web.Testing.Extensions;
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
    }
}