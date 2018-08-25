namespace Skeleton.Queues.RabbitMq.Tests.ProcessingService
{
    using Abstractions;
    using QueuesFactory.Configuration;

    public class NotificationsProcessingServiceOptions : MessagesProcessingServiceOptions
    {
        public new RabbitQueueCreationOptions QueueCreationOptions
        {
            get => (RabbitQueueCreationOptions) base.QueueCreationOptions;
            set => base.QueueCreationOptions = value;
        }
    }
}