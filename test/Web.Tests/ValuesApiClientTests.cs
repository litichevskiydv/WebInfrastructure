namespace Web.Tests
{
    using System.Collections.Generic;
    using Client;
    using Infrastructure.Integrations.WebApiClient;
    using Infrastructure.Web.Testing;
    using Infrastructure.Web.Testing.Extensions;
    using Xunit;

    public class ValuesApiClientTests : IClassFixture<BaseApiTestsFixture<TestsStartup>>
    {
        private readonly BaseApiTestsFixture<TestsStartup> _apiTestsFixture;
        private readonly ApiClient _apiClient;

        public ValuesApiClientTests(BaseApiTestsFixture<TestsStartup> apiTestsFixture)
        {
            _apiTestsFixture = apiTestsFixture;
            _apiClient = new ApiClient(new ClientConfiguration
                                       {
                                           BaseUrl = _apiTestsFixture.Server.BaseAddress.ToString(),
                                           TimeoutInMilliseconds = apiTestsFixture.TimeoutInMilliseconds
                                       },
                             _apiTestsFixture.Server.CreateHandler());
        }

        [Fact]
        public void ShouldReturnValues()
        {
            // When
            _apiClient
                .GetValues();

            // Then
            Assert.NotEmpty((IEnumerable<string>)_apiClient.CurrentState);
            _apiTestsFixture.Logger.VerifyNoErrors();
        }
    }
}