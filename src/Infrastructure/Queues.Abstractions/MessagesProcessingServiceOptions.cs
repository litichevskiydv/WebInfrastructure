namespace Skeleton.Queues.Abstractions
{
    using System;
    using QueuesFactory;

    public class MessagesProcessingServiceOptions
    {
        public TimeSpan ConnectionAttemptTimeout { get; set; }

        public QueueCreationOptions QueueCreationOptions { get; set; }
    }
}