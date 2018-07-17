namespace Web.Client.ServicesClients
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Skeleton.Web.Integration.BaseApiClient.Configuration;
    using Skeleton.Web.Serialization.Abstractions;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddValuesServiceClient(
            this IServiceCollection services, 
            IConfiguration config,
            ISerializer serializer)
        {
            if(services == null)
                throw new ArgumentNullException(nameof(services));
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            services
                .AddClient<ValuesServiceClient>()
                .ConfigureClient<ValuesServiceClientOptions>(config, builder => builder.WithSerializer(serializer));
            return services;
        }

        public static IServiceCollection AddAccountControllerClient(
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

            services
                .AddClient<AccountControllerClient>()
                .ConfigureClient<AccountControllerClientOptions>(config, builder => builder.WithSerializer(serializer));
            return services;
        }
    }
}