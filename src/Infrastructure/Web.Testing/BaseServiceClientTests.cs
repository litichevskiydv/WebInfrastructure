namespace Skeleton.Web.Testing
{
    using System;
    using Integration.BaseApiClient;
    using Integration.BaseApiClient.Configuration;
    using Moq;
    using Serialization.Jil.Configuration;
    using Serialization.Jil.Serializer;

    public class BaseServiceClientTests<TFixture, TServiceClient>
        where TFixture : BaseApiTestsFixture, new()
        where TServiceClient : BaseClient
    {
        protected readonly TFixture Fixture;
        protected readonly TServiceClient ServiceClient;

        protected BaseServiceClientTests(TFixture fixture)
        {
            Fixture = fixture;
            Fixture.MockLogger.ResetCalls();

            ServiceClient = (TServiceClient) Activator.CreateInstance(typeof(TServiceClient),
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