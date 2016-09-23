namespace Web.Tests
{
    using Client.ServicesClients;
    using Infrastructure.Integrations.WebApiClient;
    using Infrastructure.Web.Testing;
    using Infrastructure.Web.Testing.Extensions;
    using Moq;
    using Xunit;

    public class ValuesControllerTests : IClassFixture<BaseApiTestsFixture<TestsStartup>>
    {
        private readonly BaseApiTestsFixture<TestsStartup> _apiTestsFixture;
        private readonly ValuesServiceClient _valuesClient;

        public ValuesControllerTests(BaseApiTestsFixture<TestsStartup> apiTestsFixture)
        {
            _apiTestsFixture = apiTestsFixture;
            _apiTestsFixture.Logger.ResetCalls();

            _valuesClient = new ValuesServiceClient(new ClientConfiguration
                                             {
                                                 BaseUrl = _apiTestsFixture.Server.BaseAddress.ToString(),
                                                 TimeoutInMilliseconds = apiTestsFixture.TimeoutInMilliseconds
                                             },
                                _apiTestsFixture.Server.CreateHandler());
        }

        [Fact]
        public void ShouldReturnValues()
        {
            Assert.NotEmpty(_valuesClient.Get());
            _apiTestsFixture.Logger.VerifyNoErrors();
        }

        [Fact]
        public async void ShouldReturnValuesAsync()
        {
            Assert.NotEmpty(await _valuesClient.GetAsync());
            _apiTestsFixture.Logger.VerifyNoErrors();
        }
    }
}