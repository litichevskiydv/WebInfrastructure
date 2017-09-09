namespace Skeleton.Web.Testing
{
    using System;
    using Integration.BaseApiClient;
    using Moq;

    public class BaseServiceClientTests<TFixture, TServiceClient>
        where TFixture : BaseApiTestsFixture, new()
        where TServiceClient : FlurlBasedClient
    {
        protected readonly TFixture Fixture;
        protected readonly TServiceClient ServiceClient;

        protected BaseServiceClientTests(TFixture fixture)
        {
            Fixture = fixture;
            Fixture.MockLogger.ResetCalls();

            ServiceClient = (TServiceClient) Activator.CreateInstance(typeof(TServiceClient),
                Fixture.Server.CreateHandler(),
                new ClientConfiguration
                {
                    BaseUrl = Fixture.Server.BaseAddress.ToString(),
                    TimeoutInMilliseconds = Fixture.TimeoutInMilliseconds
                });
        }
    }
}