namespace Skeleton.Queues.RabbitMq.QueuesFactory.ExceptionsHandling.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class RequeuingExceptionHandler : ExceptionHandlerBase
    {
        public RequeuingExceptionHandler(ILogger<RequeuingExceptionHandler> logger) : base(logger)
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
                await Queue.RequeueMessageAsync(messageDeliveryTag, cancellationToken);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error during has occurred during handling previous exception");
            }
        }
    }
}