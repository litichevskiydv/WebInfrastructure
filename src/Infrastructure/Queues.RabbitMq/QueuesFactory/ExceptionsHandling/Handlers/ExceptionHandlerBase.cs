namespace Skeleton.Queues.RabbitMq.QueuesFactory.ExceptionsHandling.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public abstract class ExceptionHandlerBase
    {
        protected readonly ILogger Logger;
        protected RabbitQueue Queue;

        protected ExceptionHandlerBase(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            Logger = logger;
        }

        internal void Init(RabbitQueue queue)
        {
            if(queue == null)
                throw new ArgumentNullException(nameof(queue));

            Queue = queue;
        }

        public abstract Task HandleAsync(
            Exception exception,
            ulong messageDeliveryTag,
            string messageId,
            string messageContent,
            CancellationToken cancellationToken);
    }
}