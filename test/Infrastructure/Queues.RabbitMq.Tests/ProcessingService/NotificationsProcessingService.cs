namespace Skeleton.Queues.RabbitMq.Tests.ProcessingService
{
    using Abstractions;
    using Abstractions.QueuesFactory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using QueuesFactory;
    using QueuesFactory.Configuration;

    public class NotificationsProcessingService : MessagesProcessingService<string>
    {
        public NotificationsProcessingService(
            TypedQueuesFactory<RabbitMessageDescription, RabbitQueueCreationOptions> queuesFactory,
            IMessageHandler<string> messageHandler,
            ILogger<NotificationsProcessingService> logger,
            IOptions<NotificationsProcessingServiceOptions> options)
            : base(queuesFactory, messageHandler, logger, options.Value)
        {
        }
    }
}