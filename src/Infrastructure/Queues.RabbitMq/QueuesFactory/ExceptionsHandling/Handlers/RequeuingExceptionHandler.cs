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

        protected override async Task HandleExceptionAsync(
            Exception exception,
            ulong messageDeliveryTag,
            string messageId,
            string messageContent,
            CancellationToken cancellationToken)
        {
            await Queue.RequeueMessageAsync(messageDeliveryTag, cancellationToken);
        }
    }
}