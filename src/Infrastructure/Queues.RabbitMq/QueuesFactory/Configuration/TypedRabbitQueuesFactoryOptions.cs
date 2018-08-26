namespace Skeleton.Queues.RabbitMq.QueuesFactory.Configuration
{
    using System;

    public class TypedRabbitQueuesFactoryOptions
    {
        public RabbitCredentianls Credentials { get; set; }

        public string[] Hosts { get; set; }

        public TimeSpan NetworkRecoveryInterval { get; set; }
    }
}