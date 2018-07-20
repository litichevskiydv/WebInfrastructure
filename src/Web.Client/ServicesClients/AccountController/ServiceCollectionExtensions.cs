namespace Web.Client.ServicesClients.AccountController
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Skeleton.Web.Integration.BaseApiClient.Configuration;
    using Skeleton.Web.Serialization.Abstractions;

    public static class ServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddAccountControllerClient(
            this IServiceCollection services,
            IConfiguration config,
            ISerializer serializer)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            return services
                .AddClient<AccountControllerClient>()
                .ConfigureClient<AccountControllerClientOptions>(config, builder => builder.WithSerializer(serializer));
        }
    }
}