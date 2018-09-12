namespace Skeleton.Queues.RabbitMq.QueuesFactory.ExceptionsHandling.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.QueuesFactory;
    using Abstractions.QueuesFactory.ExceptionsHandling;
    using Abstractions.QueuesFactory.ExceptionsHandling.Handlers;
    using Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Thank @nikitamorgunov for sample
    /// </summary>
    public class RequeuingWithDelayExceptionHandler : ExceptionHandlerBase<RabbitMessageDescription>, IDisposable
    {
        private readonly QueueBase<RabbitMessageDescription> _pendingMessagesQueue;

        protected bool Disposed;

        public RequeuingWithDelayExceptionHandler(
            IUntypedQueuesFactory<RabbitQueueCreationOptions, QueueBase<RabbitMessageDescription>> rabbitQueuesFactory,
            ILogger<RequeuingWithDelayExceptionHandler> logger,
            string parentQueueName,
            TimeSpan messageDelay) 
            : base(logger, ExceptionHandlingPolicy.Requeue)
        {
            if(rabbitQueuesFactory == null)
                throw new ArgumentNullException(nameof(rabbitQueuesFactory));

            _pendingMessagesQueue = rabbitQueuesFactory.Create(
                new RabbitQueueCreationOptions
                {
                    QueueName = $"{parentQueueName}_PendingMessages_{messageDelay}",
                    ExceptionHandlingPolicy = ExceptionHandlingPolicy.None,
                    RetriesCount = 0,
                    RetryInitialTimeout = TimeSpan.Zero,
                    AdditionalArguments
                        = new Dictionary<string, object>
                          {
                              {"x-dead-letter-exchange", ""},
                              {"x-dead-letter-routing-key", parentQueueName},
                              {"x-message-ttl", (long) messageDelay.TotalMilliseconds},
                              {"x-expires", (long)messageDelay.TotalMilliseconds*2}
                          }
                }
            );
        }

        protected override async Task HandleExceptionAsync(
            QueueBase<RabbitMessageDescription> queue, 
            RabbitMessageDescription messageDescription, 
            Exception exception,
            CancellationToken cancellationToken)
        {

            await queue.AcknowledgeMessageAsync(messageDescription, cancellationToken);
            await _pendingMessagesQueue.SendMessageAsync(messageDescription, cancellationToken);
        }

        protected virtual void DisposeInternal(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
                _pendingMessagesQueue?.Dispose();
            Disposed = true;
        }

        public void Dispose()
        {
            DisposeInternal(true);
            GC.SuppressFinalize(this);
        }
    }
}