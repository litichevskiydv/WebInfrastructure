namespace Skeleton.Queues.RabbitMq.Tests.ProcessingService
{
    using System;
    using Abstractions.Configuration;
    using Abstractions.QueuesFactory.Configuration;
    using Abstractions.QueuesFactory.ExceptionsHandling.Handlers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using QueuesFactory;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNotificationsProcessingService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            return services
                .AddHostedService<NotificationsProcessingService>()
                .ConfigureMessagesProcessingService<NotificationsProcessingServiceOptions>(
                    configuration,
                    x => x.Configure<RequeuingExceptionHandler<RabbitMessageDescription>>(
                        (options, exceptionHandler) => options.QueueCreationOptions.WithExceptionHandler(exceptionHandler)
                    )
                );
        }
    }
}