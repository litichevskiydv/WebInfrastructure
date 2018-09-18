namespace Skeleton.Queues.RabbitMq.QueuesFactory
{
    using System.Collections.Generic;
    using Abstractions.QueuesFactory;

    public class RabbitMessageDescription : QueueMessageDescriptionBase
    {
        public IDictionary<string, object> Headers { get; set; }

        public ulong? DeliveryTag { get; set; }
    }
}