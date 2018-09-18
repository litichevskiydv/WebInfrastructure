namespace Skeleton.Queues.Abstractions.QueuesFactory.ExceptionsHandling.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using QueuesFactory;

    public class EmptyExceptionHandler<TMessageDescription> : ExceptionHandlerBase<TMessageDescription>
        where TMessageDescription : QueueMessageDescriptionBase, new()
    {
        public EmptyExceptionHandler(ILogger<EmptyExceptionHandler<TMessageDescription>> logger)
            : base(logger, ExceptionHandlingPolicy.None)
        {
        }

        protected override async Task HandleExceptionAsync(
            QueueBase<TMessageDescription> queue,
            TMessageDescription messageDescription,
            Exception exception,
            CancellationToken cancellationToken)
        {
            await queue.AcknowledgeMessageAsync(messageDescription, cancellationToken);
        }
    }
}