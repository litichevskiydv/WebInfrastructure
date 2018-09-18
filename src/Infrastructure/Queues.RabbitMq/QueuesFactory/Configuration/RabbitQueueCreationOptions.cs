namespace Skeleton.Queues.RabbitMq.QueuesFactory.Configuration
{
    using System.Collections.Generic;
    using Abstractions.QueuesFactory.Configuration;

    public class RabbitQueueCreationOptions : QueueCreationOptions<RabbitMessageDescription>
    {
        public IDictionary<string, object> AdditionalArguments { get; set; }
    }
}