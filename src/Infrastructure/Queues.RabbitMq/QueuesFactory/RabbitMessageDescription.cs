namespace Skeleton.Queues.RabbitMq.QueuesFactory
{
    using Abstractions.QueuesFactory;

    public class RabbitMessageDescription : QueueMessageDescriptionBase
    {
        public ulong? DeliveryTag { get; set; }
    }
}