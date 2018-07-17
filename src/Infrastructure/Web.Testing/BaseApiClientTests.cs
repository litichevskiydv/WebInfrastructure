namespace Skeleton.Web.Testing
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Integration.BaseApiFluentClient;
    using Moq;

    public class BaseApiClientTests<TFixture, TApiClient>
        where TFixture : BaseApiTestsFixture, new()
        where TApiClient : BaseFluentClient
    {
        protected readonly TFixture Fixture;
        protected readonly TApiClient ApiClient;
        protected dynamic AsyncApiClient => new FluentChainedTask<TApiClient>(Task.FromResult(ApiClient));

        protected BaseApiClientTests(TFixture fixture, Func<HttpClient, string, TimeSpan, TApiClient> defaultClientFactory)
        {
            Fixture = fixture;
            Fixture.MockLogger.ResetCalls();

            ApiClient = defaultClientFactory(
                Fixture.Server.CreateClient(),
                Fixture.Server.BaseAddress.ToString(),
                TimeSpan.FromMilliseconds(Fixture.TimeoutInMilliseconds)
            );
        }
    }
}