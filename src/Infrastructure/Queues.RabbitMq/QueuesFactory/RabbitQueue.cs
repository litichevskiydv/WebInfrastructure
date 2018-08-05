namespace Skeleton.Queues.RabbitMq.QueuesFactory
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.QueuesFactory;
    using ExceptionsHandling.Handlers;
    using Microsoft.Extensions.Logging;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class RabbitQueue : QueueBase
    {
        private readonly string _name;
        private readonly IConnection _connection;
        private readonly IModel _queue;

        protected readonly ExceptionHandlerBase ExceptionHandler;

        protected RabbitQueue(
            string name,
            IConnection connection,
            int retriesCount,
            TimeSpan retryInitialTimeout,
            ExceptionHandlerBase exceptionHandler,
            ILogger logger)
            : base(retriesCount, retryInitialTimeout, logger)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (exceptionHandler == null)
                throw new ArgumentNullException(nameof(exceptionHandler));

            _name = name;
            _connection = connection;

            ExceptionHandler = exceptionHandler;
            exceptionHandler.Init(this);

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

        protected override Task SendMessageAsync(string content)
        {
            ThrowIfDiposed();

            var properties = _queue.CreateBasicProperties();
            properties.Persistent = true;
            properties.MessageId = Guid.NewGuid().ToString();

            _queue.BasicPublish(
                exchange: "",
                routingKey: _name,
                basicProperties: properties,
                body: Encoding.UTF8.GetBytes(content)
            );

            return Task.CompletedTask;
        }

        protected override void Subscribe(Func<string, string, ActionForAcknowledge, ActionForExceptionHandling, Task> handleAction)
        {
            ThrowIfDiposed();

            var consumer = new AsyncEventingBasicConsumer(_queue);
            consumer.Received +=
                async (s, e) =>
                {
                    await handleAction(
                        e.BasicProperties.MessageId,
                        Encoding.UTF8.GetString(e.Body),
                        () =>
                        {
                            _queue.BasicAck(e.DeliveryTag, false);
                            return Task.CompletedTask;
                        },
                        (exception, messageId, messageContent, cancellationToken) =>
                            ExceptionHandler.HandleAsync(exception, e.DeliveryTag, messageId, messageContent, cancellationToken)
                    );
                };
        }

        protected internal async Task<RabbitQueue> AcknowledgeMessageAsync(
            ulong messageDeliveryTag, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDiposed();

            await RetryAsync(
                () =>
                {
                    _queue.BasicAck(messageDeliveryTag, false);
                    return Task.CompletedTask;
                },
                cancellationToken
            );
            return this;
        }

        protected internal async Task<RabbitQueue> RequeueMessageAsync(
            ulong messageDeliveryTag,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDiposed();

            await RetryAsync(
                () =>
                {
                    _queue.BasicNack(messageDeliveryTag, false, true);
                    return Task.CompletedTask;
                },
                cancellationToken
            );
            return this;
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

    }
}