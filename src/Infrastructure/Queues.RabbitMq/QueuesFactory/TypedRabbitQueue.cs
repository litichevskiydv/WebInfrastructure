namespace Skeleton.Queues.RabbitMq.QueuesFactory
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;
    using Abstractions.QueuesFactory;
    using Abstractions.QueuesFactory.ExceptionsHandling;
    using Abstractions.QueuesFactory.ExceptionsHandling.Handlers;
    using Microsoft.Extensions.Logging;
    using RabbitMQ.Client;

    public static class TypedRabbitQueue
    {
        public static TypedRabbitQueue<TMessage> Create<TMessage>(
            IConnection connection,
            string name,
            IDictionary<string, object> additionalArguments,
            int retriesCount,
            TimeSpan retryInitialTimeout,
            ITypedQueue<ErrorInformation> errorsQueue,
            ExceptionHandlerBase<RabbitMessageDescription> exceptionHandler,
            ILogger<TypedRabbitQueue<TMessage>> logger)
        {
            try
            {
                return new TypedRabbitQueue<TMessage>(connection, name, additionalArguments, retriesCount, retryInitialTimeout, errorsQueue, exceptionHandler, logger);
            }
            catch (Exception)
            {
                connection?.Dispose();
                errorsQueue?.Dispose();
                throw;
            }
        }
    }

    public class TypedRabbitQueue<TMessage> : RabbitQueue, ITypedQueue<TMessage>
    {
        internal TypedRabbitQueue(
            IConnection connection,
            string name,
            IDictionary<string, object> additionalArguments,
            int retriesCount,
            TimeSpan retryInitialTimeout,
            ITypedQueue<ErrorInformation> errorsQueue,
            ExceptionHandlerBase<RabbitMessageDescription> exceptionHandler,
            ILogger<TypedRabbitQueue<TMessage>> logger)
            : base(connection, name, additionalArguments, retriesCount, retryInitialTimeout, errorsQueue, exceptionHandler, logger)
        {
        }

        public async Task<ITypedQueue<TMessage>> SendMessageAsync(
            TMessage message, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.SendMessageAsync(message, cancellationToken);
            return this;
        }

        public async Task<ITypedQueue<TMessage>> SubscribeAsync(
            IMessageHandler<TMessage> handler, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.SubscribeAsync(handler, cancellationToken);
            return this;
        }
    }
}