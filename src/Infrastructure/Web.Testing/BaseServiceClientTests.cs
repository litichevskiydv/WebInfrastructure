namespace Skeleton.Web.Testing
{
    using System;
    using System.Net.Http;
    using Integration.BaseApiClient;
    using Moq;

    public class BaseServiceClientTests<TStartup, TServiceClient>
        where TStartup : class
        where TServiceClient : BaseClient
    {
        protected readonly BaseApiTestsFixture<TStartup> Fixture;
        protected readonly TServiceClient ServiceClient;

        protected BaseServiceClientTests(
            BaseApiTestsFixture<TStartup> fixture, 
            Func<HttpClient, string, TimeSpan, TServiceClient> defaultClientFactory)
        {
            Fixture = fixture;
            Fixture.MockLogger.ResetCalls();

            ServiceClient = defaultClientFactory(
                Fixture.CreateClient(),
                Fixture.ClientOptions.BaseAddress.ToString(),
                Fixture.ApiTimeout
            );
        }
    }
}