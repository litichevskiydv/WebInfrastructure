namespace Skeleton.Queues.RabbitMq.Tests.ProcessingService
{
    using Abstractions;
    using QueuesFactory.Configuration;

    public class NotificationsProcessingServiceOptions : MessagesProcessingServiceOptions<RabbitQueueCreationOptions>
    {
    }
}