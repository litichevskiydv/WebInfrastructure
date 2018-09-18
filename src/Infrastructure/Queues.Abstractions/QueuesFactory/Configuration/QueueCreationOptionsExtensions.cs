namespace Skeleton.Queues.Abstractions.QueuesFactory.Configuration
{
    using System;
    using ExceptionsHandling.Handlers;

    public static class QueueCreationOptionsExtensions
    {
        public static TQueueCreationOptions WithExceptionHandler<TQueueCreationOptions, TMessageDescription>(
            this TQueueCreationOptions queueCreationOptions,
            ExceptionHandlerBase<TMessageDescription> exceptionHandler)
            where TQueueCreationOptions : QueueCreationOptions<TMessageDescription>
            where TMessageDescription : QueueMessageDescriptionBase, new()
        {
            if(queueCreationOptions == null)
                throw new ArgumentNullException(nameof(queueCreationOptions));
            if (exceptionHandler == null)
                throw new ArgumentNullException(nameof(exceptionHandler));

            queueCreationOptions.ExceptionHandler = exceptionHandler;
            return queueCreationOptions;
        }
    }
}