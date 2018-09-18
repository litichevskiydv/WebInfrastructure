namespace Skeleton.Queues.Abstractions.QueuesFactory.ExceptionsHandling.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using QueuesFactory;

    public class RequeuingExceptionHandler<TMessageDescription> : ExceptionHandlerBase<TMessageDescription>
        where TMessageDescription : QueueMessageDescriptionBase, new()
    {
        public RequeuingExceptionHandler(ILogger<RequeuingExceptionHandler<TMessageDescription>> logger)
            : base(logger, ExceptionHandlingPolicy.Requeue)
        {
        }

        protected override async Task HandleExceptionAsync(
            QueueBase<TMessageDescription> queue,
            TMessageDescription messageDescription,
            Exception exception,
            CancellationToken cancellationToken)
        {
            await queue.RejectMessageAsync(messageDescription, cancellationToken);
        }
    }
}