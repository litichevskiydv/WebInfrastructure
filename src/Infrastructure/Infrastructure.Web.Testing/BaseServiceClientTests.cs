namespace Infrastructure.Web.Testing
{
    using System;
    using Integrations.WebApiClient;
    using Moq;
    using Xunit;

    public class BaseServiceClientTests<TFixture, TServiceClient> : IClassFixture<TFixture>
        where TFixture : BaseApiTestsFixture
        where TServiceClient : BaseClient
    {
        protected readonly TFixture Fixture;
        protected readonly TServiceClient ServiceClient;

        protected BaseServiceClientTests(TFixture fixture)
        {
            Fixture = fixture;
            Fixture.Logger.ResetCalls();

            ServiceClient = (TServiceClient) Activator.CreateInstance(typeof(TServiceClient),
                new ClientConfiguration
                {
                    BaseUrl = Fixture.Server.BaseAddress.ToString(),
                    TimeoutInMilliseconds = Fixture.TimeoutInMilliseconds
                },
                Fixture.Server.CreateHandler());
        }
    }
}