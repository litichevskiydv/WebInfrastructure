namespace Skeleton.Queues.Abstractions.Configuration
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureMessagesProcessingService<TServiceOptions>(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<OptionsBuilder<TServiceOptions>> optionsConfigurator)
            where TServiceOptions : MessagesProcessingServiceOptions
        {
            if(services == null)
                throw new ArgumentNullException(nameof(services));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if (optionsConfigurator == null)
                throw new ArgumentNullException(nameof(optionsConfigurator));

            services.Configure<TServiceOptions>(configuration);
            optionsConfigurator(services.AddOptions<TServiceOptions>());

            return services;
        }
    }
}