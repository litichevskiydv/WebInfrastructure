namespace Infrastructure.Web.Testing
{
    using System;
    using Integrations.WebApiClient;
    using Moq;
    using Xunit;

    public class BaseApiClientTests<TFixture, TApiClient> : IClassFixture<TFixture>
        where TFixture : BaseApiTestsFixture
        where TApiClient : BaseFluentClient
    {
        protected readonly TFixture Fixture;
        protected readonly TApiClient ApiClient;

        public BaseApiClientTests(TFixture fixture)
        {
            Fixture = fixture;
            Fixture.Logger.ResetCalls();

            ApiClient = (TApiClient)Activator.CreateInstance(typeof(TApiClient),
                new ClientConfiguration
                {
                    BaseUrl = Fixture.Server.BaseAddress.ToString(),
                    TimeoutInMilliseconds = Fixture.TimeoutInMilliseconds
                },
                Fixture.Server.CreateHandler());
        }
    }
}