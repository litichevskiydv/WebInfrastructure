namespace Skeleton.Queues.RabbitMq.QueuesFactory
{
    using System;
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
            string name,
            IConnection connection,
            int retriesCount,
            TimeSpan retryInitialTimeout,
            ITypedQueue<ExceptionDescription> errorsQueue,
            ExceptionHandlerBase<RabbitMessageDescription> exceptionHandler,
            ILogger logger)
            : base(retriesCount, retryInitialTimeout, errorsQueue, exceptionHandler, logger)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            _name = name;
            _connection = connection;

            _queue = _connection.CreateModel();
            _queue.QueueDeclare(
                queue: name,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            _queue.BasicQos(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false
            );
        }

        private void ThrowIfDiposed()
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        protected override Task SendMessageAsync(
            RabbitMessageDescription messageDescription, 
            CancellationToken cancellationToken)
        {
            ThrowIfDiposed();

            var properties = _queue.CreateBasicProperties();
            properties.Persistent = true;
            properties.MessageId = messageDescription.Id;

            _queue.BasicPublish(
                exchange: "",
                routingKey: _name,
                basicProperties: properties,
                body: Encoding.UTF8.GetBytes(messageDescription.Content)
            );

            return Task.CompletedTask;
        }

        protected override Task SubscribeAsync(
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
                            Content = Encoding.UTF8.GetString(e.Body),
                            DeliveryTag = e.DeliveryTag
                        }
                    );

            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                _queue?.Dispose();
                _connection?.Dispose();
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