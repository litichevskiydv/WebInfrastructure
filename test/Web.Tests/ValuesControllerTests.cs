namespace Web.Tests
{
    using Client;
    using Infrastructure.Integrations.WebApiClient;
    using Infrastructure.Web.Testing.Extensions;
    using Moq;
    using Xunit;

    public class ValuesControllerTests : IClassFixture<TestsFixture>
    {
        private readonly TestsFixture _apiTestsFixture;
        private readonly ValuesClient _valuesClient;

        public ValuesControllerTests(TestsFixture apiTestsFixture)
        {
            _apiTestsFixture = apiTestsFixture;
            _apiTestsFixture.Logger.ResetCalls();

            _valuesClient = new ValuesClient(new ClientConfiguration
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