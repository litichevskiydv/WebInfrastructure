namespace Skeleton.Queues.Abstractions.QueuesFactory.ExceptionsHandling.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using QueuesFactory;

    public class ErrorsQueuingExceptionHandler<TMessageDescription> : ExceptionHandlerBase<TMessageDescription>
        where TMessageDescription : QueueMessageDescriptionBase, new()
    {
        public ErrorsQueuingExceptionHandler(ILogger<ErrorsQueuingExceptionHandler<TMessageDescription>> logger)
            : base(logger, ExceptionHandlingPolicy.SendToErrorsQueue)
        {
        }

        protected override async Task HandleExceptionAsync(
            QueueBase<TMessageDescription> queue,
            TMessageDescription messageDescription,
            Exception exception,
            CancellationToken cancellationToken)
        {

            await queue.AcknowledgeMessageAsync(messageDescription, cancellationToken);
            await queue.ErrorsQueue.SendMessageAsync(
                new ErrorInformation
                {
                    ExceptionDescription = exception.ToString(),
                    MessageId = messageDescription.Id,
                    MessageContent = messageDescription.Content
                },
                cancellationToken
            );
        }
    }
}