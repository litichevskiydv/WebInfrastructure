namespace Skeleton.Queues.Abstractions.QueuesFactory.Configuration
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

        internal QueueCreationOptions()
        {
        }
    }

    public abstract class QueueCreationOptions<TMessageDescription> : QueueCreationOptions
        where TMessageDescription : QueueMessageDescriptionBase, new()
    {
        public ExceptionHandlerBase<TMessageDescription> ExceptionHandler { get; set; }
    }
}