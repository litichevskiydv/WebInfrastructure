namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    using System;
    using ExceptionsHandling;

    public class QueueCreationOptions
    {
        public string QueueName { get; set; }

        public int RetriesCount { get; set; }

        public TimeSpan RetryInitialTimeout { get; set; }

        public ExceptionHandlingPolicy ExceptionHandlingPolicy { get; set; }
    }
}