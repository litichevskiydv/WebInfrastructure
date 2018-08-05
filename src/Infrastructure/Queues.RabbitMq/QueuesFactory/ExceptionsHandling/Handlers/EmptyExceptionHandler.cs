namespace Skeleton.Queues.RabbitMq.QueuesFactory.ExceptionsHandling.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class EmptyExceptionHandler : ExceptionHandlerBase
    {
        public EmptyExceptionHandler(ILogger<EmptyExceptionHandler> logger) : base(logger)
        {
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
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error during has occurred during handling previous exception");
            }
        }
    }
}