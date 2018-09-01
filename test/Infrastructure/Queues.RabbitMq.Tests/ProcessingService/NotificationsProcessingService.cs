namespace Skeleton.Queues.RabbitMq.Tests.ProcessingService
{
    using Abstractions;
    using Abstractions.QueuesFactory;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [UsedImplicitly]
    public class NotificationsProcessingService : MessagesProcessingService<string>
    {
        public NotificationsProcessingService(
            IGenericQueuesFactory queuesFactory,
            IMessageHandler<string> messageHandler,
            ILogger<NotificationsProcessingService> logger,
            IOptions<NotificationsProcessingServiceOptions> options)
            : base(queuesFactory, messageHandler, logger, options.Value)
        {
        }
    }
}