namespace Skeleton.Queues.RabbitMq.Tests.ProcessingService
{
    using System;
    using Abstractions.Configuration;
    using Abstractions.QueuesFactory;
    using Abstractions.QueuesFactory.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using QueuesFactory;
    using QueuesFactory.Configuration;
    using QueuesFactory.ExceptionsHandling.Handlers;

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
                    x => x
                        .Configure<IUntypedQueuesFactory<RabbitQueueCreationOptions, QueueBase<RabbitMessageDescription>>,
                            ILogger<RequeuingWithDelayExceptionHandler>>(
                            (options, queuesFactory, logger) =>
                                options.QueueCreationOptions.WithExceptionHandler(
                                    new RequeuingWithDelayExceptionHandler(
                                        queuesFactory,
                                        logger,
                                        options.QueueCreationOptions.QueueName,
                                        options.MessagesRequeuingDelay
                                    )
                                )
                        )
                );
        }
    }
}