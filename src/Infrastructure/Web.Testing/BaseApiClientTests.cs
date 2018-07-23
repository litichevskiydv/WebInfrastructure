namespace Skeleton.Web.Testing
{
    using System;
    using System.Threading.Tasks;
    using Integration.BaseApiClient.Configuration;
    using Integration.BaseApiFluentClient;
    using Microsoft.Extensions.Options;
    using Moq;
    using Serialization.Jil.Serializer;

    public class BaseApiClientTests<TStartup> where TStartup : class
    {
        protected readonly BaseApiTestsFixture<TStartup> Fixture;

        protected BaseApiClientTests(BaseApiTestsFixture<TStartup> fixture)
        {
            Fixture = fixture;
            Fixture.MockLogger.ResetCalls();
        }

        protected TClient CreateClient<TClient, TClientOptions>(Action<TClientOptions> configureOptions = null)
            where TClient : BaseFluentClient
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

            return (TClient)Activator.CreateInstance(
                typeof(TClient),
                Fixture.CreateClient(),
                Options.Create(clientOptions)
            );
        }

        protected dynamic CreateAsyncClient<TClient>(TClient client) where TClient : BaseFluentClient =>
            new FluentChainedTask<TClient>(Task.FromResult(client));
    }
}