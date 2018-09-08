namespace Skeleton.Queues.RabbitMq.QueuesFactory
{
    using System;
    using Abstractions.QueuesFactory;
    using Abstractions.QueuesFactory.ExceptionsHandling;
    using Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using RabbitMQ.Client;

    public class TypedRabbitQueuesFactory : ITypedQueuesFactory<RabbitQueueCreationOptions>
    {
        private readonly IExceptionHandlersFactory<RabbitMessageDescription> _exceptionHandlersFactory;
        private readonly ILoggerFactory _loggerFactory;

        private readonly string[] _hosts;
        private readonly IConnectionFactory _connectionsFactory;

        public TypedRabbitQueuesFactory(
            IExceptionHandlersFactory<RabbitMessageDescription> exceptionHandlersFactory, 
            ILoggerFactory loggerFactory, 
            IOptions<TypedRabbitQueuesFactoryOptions> options)
        {
            if (exceptionHandlersFactory == null)
                throw new ArgumentNullException(nameof(exceptionHandlersFactory));
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));
            if(options == null)
                throw new ArgumentNullException(nameof(options));

            _exceptionHandlersFactory = exceptionHandlersFactory;
            _loggerFactory = loggerFactory;

            _hosts = options.Value.Hosts;
            _connectionsFactory
                = new ConnectionFactory
                  {
                      UserName = options.Value.Credentials.UserName,
                      Password = options.Value.Credentials.Password,
                      AutomaticRecoveryEnabled = true,
                      NetworkRecoveryInterval = options.Value.NetworkRecoveryInterval,
                      TopologyRecoveryEnabled = true,
                      DispatchConsumersAsync = true
                  };
        }

        protected virtual IConnection CreateConnection(string[] hosts)
        {
            return _connectionsFactory.CreateConnection(hosts);
        }

        protected virtual ITypedQueue<ExceptionDescription> CreateErrorsQueue(RabbitQueueCreationOptions parentQueueCreationOptions)
        {
            return Create<ExceptionDescription>(
                new RabbitQueueCreationOptions
                {
                    QueueName = $"{parentQueueCreationOptions.QueueName}.Errors",
                    RetriesCount = parentQueueCreationOptions.RetriesCount,
                    RetryInitialTimeout = parentQueueCreationOptions.RetryInitialTimeout,
                    ExceptionHandlingPolicy = ExceptionHandlingPolicy.None
                }
            );
        }

        public ITypedQueue<TMessage> Create<TMessage>(RabbitQueueCreationOptions creationOptions)
        {
            if (creationOptions == null)
                throw new ArgumentNullException(nameof(creationOptions));
            if (creationOptions.ExceptionHandlingPolicy.HasValue == false && creationOptions.ExceptionHandler == null)
                throw new ArgumentException(
                    $"{nameof(RabbitQueueCreationOptions.ExceptionHandlingPolicy)} " +
                    $"and {creationOptions.ExceptionHandler} can't be null simultaneously"
                );

            return TypedRabbitQueue.Create(
                CreateConnection(_hosts),
                creationOptions.QueueName,
                creationOptions.AdditionalArguments,
                creationOptions.RetriesCount,
                creationOptions.RetryInitialTimeout,
                creationOptions.ExceptionHandlingPolicy == ExceptionHandlingPolicy.SendToErrorsQueue
                    ? CreateErrorsQueue(creationOptions)
                    : null,
                creationOptions.ExceptionHandler
                ?? _exceptionHandlersFactory.GetHandler(creationOptions.ExceptionHandlingPolicy.Value),
                _loggerFactory.CreateLogger<TypedRabbitQueue<TMessage>>()
            );
        }
    }
}