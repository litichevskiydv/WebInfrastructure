namespace Skeleton.Queues.RabbitMq.QueuesFactory
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.QueuesFactory;
    using Abstractions.QueuesFactory.ExceptionsHandling;
    using Abstractions.QueuesFactory.ExceptionsHandling.Handlers;
    using Microsoft.Extensions.Logging;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class RabbitQueue : QueueBase<RabbitMessageDescription>
    {
        private readonly string _name;
        private readonly IConnection _connection;
        private readonly IModel _queue;

        protected RabbitQueue(
            IConnection connection,
            string name,
            IDictionary<string, object> additionalArguments,
            int retriesCount,
            TimeSpan retryInitialTimeout,
            ITypedQueue<ErrorInformation> errorsQueue,
            ExceptionHandlerBase<RabbitMessageDescription> exceptionHandler,
            ILogger logger)
            : base(retriesCount, retryInitialTimeout, errorsQueue, exceptionHandler, logger)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            _name = name;
            _connection = connection;

            _queue = _connection.CreateModel();
            try
            {
                _queue.QueueDeclare(queue: name, durable: true, exclusive: false, autoDelete: false, arguments: additionalArguments);
                _queue.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            }
            catch (Exception)
            {
                _queue.Dispose();
                throw;
            }
        }

        private void ThrowIfDiposed()
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        protected override Task SendMessageInternalAsync(
            RabbitMessageDescription messageDescription, 
            CancellationToken cancellationToken)
        {
            ThrowIfDiposed();

            var properties = _queue.CreateBasicProperties();
            properties.Persistent = true;
            properties.MessageId = messageDescription.Id;
            properties.Headers = messageDescription.Headers;

            _queue.BasicPublish(
                exchange: "",
                routingKey: _name,
                basicProperties: properties,
                body: Encoding.UTF8.GetBytes(messageDescription.Content)
            );

            return Task.CompletedTask;
        }

        protected override Task SubscribeInternalAsync(
            Func<RabbitMessageDescription, Task> messageHandler, 
            CancellationToken cancellationToken)
        {
            ThrowIfDiposed();

            var consumer = new AsyncEventingBasicConsumer(_queue);
            consumer.Received +=
                async (s, e) =>
                    await messageHandler(
                        new RabbitMessageDescription
                        {
                            Id = e.BasicProperties.MessageId,
                            Headers = e.BasicProperties.Headers,
                            Content = Encoding.UTF8.GetString(e.Body.ToArray()),
                            DeliveryTag = e.DeliveryTag
                        }
                    );
            _queue.BasicConsume(_name, false, consumer);

            return Task.CompletedTask;
        }

        protected override void DisposeInternal(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                _queue?.Dispose();
                _connection?.Dispose();
                ErrorsQueue?.Dispose();
                (ExceptionHandler as IDisposable)?.Dispose();
            }
            Disposed = true;
        }

        public override async Task AcknowledgeMessageAsync(
            RabbitMessageDescription messageDescription, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDiposed();

            await RetryAsync(
                () =>
                {
                    _queue.BasicAck(messageDescription.DeliveryTag.Value, false);
                    return Task.CompletedTask;
                },
                cancellationToken
            );
        }

        public override async Task RejectMessageAsync(
            RabbitMessageDescription messageDescription,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDiposed();

            await RetryAsync(
                () =>
                {
                    _queue.BasicNack(messageDescription.DeliveryTag.Value, false, true);
                    return Task.CompletedTask;
                },
                cancellationToken
            );
        }
    }
}