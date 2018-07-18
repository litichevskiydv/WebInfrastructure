namespace Web.Tests
{
    using Client.ServicesClients.AccountController;
    using Client.ServicesClients.ValuesService;
    using Microsoft.Extensions.DependencyInjection;
    using Skeleton.Web.Serialization.Jil.Serializer;
    using Skeleton.Web.Testing;
    using Skeleton.Web.Testing.Extensions;
    using Xunit;

    [Collection(nameof(ApiTestsCollection))]
    public class ServicesClientsTests
    {
        private readonly BaseApiTestsFixture<Startup> _fixture;

        public ServicesClientsTests(BaseApiTestsFixture<Startup> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ServiceProviderShouldCreateValuesServiceClient()
        {
            // Given
            var serviceProvider = new ServiceCollection()
                .AddValuesServiceClient(_fixture.Configuration.GetSection("ValuesServiceClientOptions"), JilSerializer.Default)
                .ConfigurePrimaryHttpMessageHandler(() => _fixture.Server.CreateHandler())
                .Services.BuildServiceProvider();

            // When
            var values = serviceProvider.GetService<IValuesServiceClient>().Get();

            // Then
            Assert.NotEmpty(values);

            _fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyNoWarningsWasLogged();
        }

        [Fact]
        public void ServiceProviderShouldCreateAccountControllerClient()
        {
            // Given
            const string login = "lhp@lhp.com";
            const string password = "1234";

            var serviceProvider = new ServiceCollection()
                .AddAccountControllerClient(_fixture.Configuration.GetSection("AccountControllerClientOptions"), JilSerializer.Default)
                .ConfigurePrimaryHttpMessageHandler(() => _fixture.Server.CreateHandler())
                .Services.BuildServiceProvider();

            // When
            var tokenResponse = serviceProvider.GetService<AccountControllerClient>().Token(login, password);

            // Then
            Assert.NotNull(tokenResponse);

            _fixture.MockLogger
                .VerifyNoErrorsWasLogged()
                .VerifyNoWarningsWasLogged();
        }
    }
}