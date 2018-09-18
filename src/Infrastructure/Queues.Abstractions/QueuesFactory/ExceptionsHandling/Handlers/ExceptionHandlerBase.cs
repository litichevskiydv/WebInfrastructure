namespace Skeleton.Queues.Abstractions.QueuesFactory.ExceptionsHandling.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public abstract class ExceptionHandlerBase<TMessageDescription> 
        where TMessageDescription : QueueMessageDescriptionBase, new()
    {
        protected readonly ILogger Logger;
        public ExceptionHandlingPolicy ExceptionHandlingPolicy { get; }

        protected ExceptionHandlerBase(ILogger logger, ExceptionHandlingPolicy exceptionHandlingPolicy)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            Logger = logger;
            ExceptionHandlingPolicy = exceptionHandlingPolicy;
        }

        protected abstract Task HandleExceptionAsync(
            QueueBase<TMessageDescription> queue,
            TMessageDescription messageDescription,
            Exception exception,
            CancellationToken cancellationToken);

        public async Task HandleAsync(
            QueueBase<TMessageDescription> queue, 
            TMessageDescription messageDescription, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            try
            {
                await HandleExceptionAsync(queue, messageDescription, exception, cancellationToken);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error during has occurred during handling previous exception");
            }
        }
    }
}