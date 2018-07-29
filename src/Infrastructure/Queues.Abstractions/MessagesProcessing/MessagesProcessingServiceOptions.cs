namespace Skeleton.Queues.Abstractions.MessagesProcessing
{
    using System;

    public class MessagesProcessingServiceOptions
    {
        public string QueueName { get; set; }

        public TimeSpan RetryInitialTimeout { get; set; }
    }
}