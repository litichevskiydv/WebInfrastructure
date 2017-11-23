namespace Skeleton.Web.Testing
{
    using System;
    using System.Threading.Tasks;
    using Integration.BaseApiClient.Configuration;
    using Integration.BaseApiFluentClient;
    using Moq;
    using Serialization.Jil.Configuration;
    using Serialization.Jil.Serializer;

    public class BaseApiClientTests<TFixture, TApiClient>
        where TFixture : BaseApiTestsFixture, new()
        where TApiClient : BaseFluentClient
    {
        protected readonly TFixture Fixture;
        protected readonly TApiClient ApiClient;
        protected dynamic AsyncApiClient => new FluentChainedTask<TApiClient>(Task.FromResult(ApiClient));

        protected BaseApiClientTests(TFixture fixture)
        {
            Fixture = fixture;
            Fixture.MockLogger.ResetCalls();

            ApiClient = (TApiClient) Activator.CreateInstance(typeof(TApiClient),
                new Func<ClientConfiguration, ClientConfiguration>(
                    x => x.WithBaseUrl(Fixture.Server.BaseAddress.ToString())
                        .WithTimeout(TimeSpan.FromMilliseconds(Fixture.TimeoutInMilliseconds))
                        .WithHttpMessageHandler(Fixture.Server.CreateHandler())
                        .WithSerializer(new JilSerializer(OptionsExtensions.Default))
                )
            );
        }
    }
}