namespace Web.Tests
{
    using Client;
    using Infrastructure.Integrations.WebApiClient;
    using Moq;
    using Xunit;

    public class ValuesControllerTests : IClassFixture<ApiTestsFixture>
    {
        private readonly ApiTestsFixture _apiTestsFixture;
        private readonly ValuesClient _valuesClient;

        public ValuesControllerTests(ApiTestsFixture apiTestsFixture)
        {
            _apiTestsFixture = apiTestsFixture;
            _apiTestsFixture.Logger.ResetCalls();

            _valuesClient = new ValuesClient(new ClientConfiguration
                                             {
                                                 BaseUrl = _apiTestsFixture.Server.BaseAddress.ToString(),
                                                 TimeoutInMilliseconds = apiTestsFixture.TimeoutInMilliseconds
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
