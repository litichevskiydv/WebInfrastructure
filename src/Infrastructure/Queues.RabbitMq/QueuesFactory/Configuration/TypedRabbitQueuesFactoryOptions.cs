namespace Skeleton.Queues.RabbitMq.QueuesFactory.Configuration
{
    using System;

    public class TypedRabbitQueuesFactoryOptions
    {
        public RabbitCredentianls Credentianls { get; set; }

        public string[] Hosts { get; set; }

        public TimeSpan NetworkRecoveryInterval { get; set; }
    }
}