namespace Skeleton.Queues.RabbitMq.QueuesFactory.ExceptionsHandling.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.QueuesFactory;
    using Abstractions.QueuesFactory.ExceptionsHandling;
    using Microsoft.Extensions.Logging;

    public class ErrorsQueuingExceptionHandler : ExceptionHandlerBase
    {
        private readonly ITypedQueue<ExceptionDescription> _errorsQueue;

        public ErrorsQueuingExceptionHandler(
            ITypedQueue<ExceptionDescription> errorsQueue,
            ILogger<ErrorsQueuingExceptionHandler> logger)
            : base(logger)
        {
            if (errorsQueue == null)
                throw new ArgumentNullException(nameof(errorsQueue));

            _errorsQueue = errorsQueue;
        }

        public override async Task HandleAsync(
            Exception exception,
            ulong messageDeliveryTag,
            string messageId,
            string messageContent,
            CancellationToken cancellationToken)
        {
            try
            {
                await Queue.AcknowledgeMessageAsync(messageDeliveryTag, cancellationToken);
                await _errorsQueue.SendMessageAsync(
                    new ExceptionDescription
                    {
                        ExceptionMessage = exception.Message,
                        MessageId = messageId,
                        MessageContent = messageContent
                    },
                    cancellationToken
                );
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error during has occurred during handling previous exception");
            }
        }
    }
}