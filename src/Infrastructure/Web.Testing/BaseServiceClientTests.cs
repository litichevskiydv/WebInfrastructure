namespace Skeleton.Web.Testing
{
    using System;
    using System.Net.Http;
    using Integration.BaseApiClient;
    using Moq;

    public class BaseServiceClientTests<TFixture, TServiceClient>
        where TFixture : BaseApiTestsFixture, new()
        where TServiceClient : BaseClient
    {
        protected readonly TFixture Fixture;
        protected readonly TServiceClient ServiceClient;

        protected BaseServiceClientTests(TFixture fixture, Func<HttpClient, string, TimeSpan, TServiceClient> defaultClientFactory)
        {
            Fixture = fixture;
            Fixture.MockLogger.ResetCalls();

            ServiceClient = defaultClientFactory(
                Fixture.Server.CreateClient(),
                Fixture.Server.BaseAddress.ToString(),
                Fixture.ApiTimeout
            );
        }
    }
}