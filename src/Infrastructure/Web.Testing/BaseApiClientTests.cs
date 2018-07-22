namespace Skeleton.Web.Testing
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Integration.BaseApiFluentClient;
    using Moq;

    public class BaseApiClientTests<TStartup, TApiClient>
        where TStartup : class
        where TApiClient : BaseFluentClient
    {
        protected readonly BaseApiTestsFixture<TStartup> Fixture;
        protected readonly TApiClient ApiClient;
        protected dynamic AsyncApiClient => new FluentChainedTask<TApiClient>(Task.FromResult(ApiClient));

        protected BaseApiClientTests(
            BaseApiTestsFixture<TStartup> fixture, 
            Func<HttpClient, string, TimeSpan, TApiClient> defaultClientFactory)
        {
            Fixture = fixture;
            Fixture.MockLogger.ResetCalls();

            ApiClient = defaultClientFactory(
                Fixture.CreateClient(),
                Fixture.ClientOptions.BaseAddress.ToString(),
                Fixture.ApiTimeout
            );
        }
    }
}