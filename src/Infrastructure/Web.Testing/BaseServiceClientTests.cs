namespace Skeleton.Web.Testing
{
    using System;
    using Integration.BaseApiClient;
    using Integration.BaseApiClient.Configuration;
    using Moq;
    using Newtonsoft.Json;
    using Serialization.JsonNet.Configuration;

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
                new Func<IClientConfigurator, IClientConfigurator>(
                    x => x.WithBaseUrl(Fixture.Server.BaseAddress.ToString())
                        .WithTimeout(TimeSpan.FromMilliseconds(Fixture.TimeoutInMilliseconds))
                        .WithHttpMessageHandler(Fixture.Server.CreateHandler())
                        .WithJsonNetSerializer(new JsonSerializerSettings().UseDefaultSettings())
                )
            );
        }
    }
}