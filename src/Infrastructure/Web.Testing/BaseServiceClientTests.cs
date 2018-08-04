namespace Skeleton.Web.Testing
{
    using System;
    using Hosting;
    using Integration.BaseApiClient;
    using Integration.BaseApiClient.Configuration;
    using Microsoft.Extensions.Options;
    using Moq;
    using Serialization.Jil.Serializer;

    public class BaseServiceClientTests<TStartup> where TStartup : WebApiBaseStartup
    {
        protected readonly BaseApiTestsFixture<TStartup> Fixture;

        protected BaseServiceClientTests(BaseApiTestsFixture<TStartup> fixture)
        {
            Fixture = fixture;
            Fixture.MockLogger.ResetCalls();
        }

        protected TClient CreateClient<TClient, TClientOptions>(Action<TClientOptions> configureOptions = null)
            where TClient : BaseClient
            where TClientOptions : BaseClientOptions, new()
        {
            var clientOptions
                = new TClientOptions
                  {
                      BaseUrl = Fixture.ClientOptions.BaseAddress.ToString(),
                      Timeout = Fixture.ApiTimeout,
                      Serializer = JilSerializer.Default
                  };
            configureOptions?.Invoke(clientOptions);

            return (TClient) Activator.CreateInstance(
                typeof(TClient),
                Fixture.CreateClient(),
                Options.Create(clientOptions)
            );
        }
    }
}