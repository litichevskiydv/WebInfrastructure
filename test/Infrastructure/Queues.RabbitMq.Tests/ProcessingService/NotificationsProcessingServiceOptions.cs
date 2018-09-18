namespace Skeleton.Queues.RabbitMq.Tests.ProcessingService
{
    using System;
    using Abstractions;
    using QueuesFactory.Configuration;

    public class NotificationsProcessingServiceOptions : MessagesProcessingServiceOptions<RabbitQueueCreationOptions>
    {
        public TimeSpan MessagesRequeuingDelay { get; set; }
    }
}