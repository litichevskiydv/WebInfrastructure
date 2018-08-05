namespace Skeleton.Queues.RabbitMq.QueuesFactory
{
    using System;
    using Abstractions.QueuesFactory;
    using ExceptionsHandling;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using RabbitMQ.Client;

    public class TypedRabbitQueuesFactory : ITypedQueuesFactory<RabbitQueue>
    {
        private readonly IExceptionHandlersFactory _exceptionHandlersFactory;
        private readonly ILoggerFactory _loggerFactory;

        private readonly string[] _hosts;
        private readonly IConnectionFactory _connectionsFactory;

        public TypedRabbitQueuesFactory(
            IExceptionHandlersFactory exceptionHandlersFactory, 
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
                      UserName = options.Value.Credentianls.UserName,
                      Password = options.Value.Credentianls.Password,
                      AutomaticRecoveryEnabled = true,
                      NetworkRecoveryInterval = options.Value.NetworkRecoveryInterval,
                      TopologyRecoveryEnabled = true
                  };
        }

        public ITypedQueue<TMessage> Create<TMessage>(QueueCreationOptions creationOptions)
        {
            var connection = _connectionsFactory.CreateConnection(_hosts);
            return new TypedRabbitQueue<TMessage>(
                creationOptions.QueueName,
                connection,
                creationOptions.RetriesCount,
                creationOptions.RetryInitialTimeout,
                _exceptionHandlersFactory.CreateHandler(creationOptions),
                _loggerFactory.CreateLogger<TypedRabbitQueue<TMessage>>()
            );
        }
    }
}