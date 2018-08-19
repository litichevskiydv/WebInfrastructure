namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    using System;
    using ExceptionsHandling;
    using ExceptionsHandling.Handlers;

    public abstract class QueueCreationOptions
    {
        public string QueueName { get; set; }

        public int RetriesCount { get; set; }

        public TimeSpan RetryInitialTimeout { get; set; }

        public ExceptionHandlingPolicy? ExceptionHandlingPolicy { get; set; }
    }

    public abstract class QueueCreationOptions<TMessageDescription> : QueueCreationOptions
        where TMessageDescription : QueueMessageDescriptionBase, new()
    {
        public ExceptionHandlerBase<TMessageDescription> ExceptionHandler { get; set; }
    }
}