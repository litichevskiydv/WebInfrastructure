namespace Skeleton.Queues.RabbitMq.QueuesFactory.ExceptionsHandling
{
    using System;
    using Abstractions.QueuesFactory;
    using Abstractions.QueuesFactory.ExceptionsHandling;
    using Handlers;
    using Microsoft.Extensions.Logging;

    public class ExceptionHandlersFactory : IExceptionHandlersFactory
    {
        private readonly ITypedQueuesFactory<RabbitQueue> _queuesFactory;
        private readonly ILoggerFactory _loggerFactory;

        public ExceptionHandlersFactory(ITypedQueuesFactory<RabbitQueue> queuesFactory, ILoggerFactory loggerFactory)
        {
            if(queuesFactory == null)
                throw new ArgumentNullException(nameof(queuesFactory));
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            _queuesFactory = queuesFactory;
            _loggerFactory = loggerFactory;
        }

        public ExceptionHandlerBase CreateHandler(QueueCreationOptions queueCreationOptions)
        {
            switch (queueCreationOptions.ExceptionHandlingPolicy)
            {
                case ExceptionHandlingPolicy.Requeue:
                    return new RequeuingExceptionHandler(_loggerFactory.CreateLogger<RequeuingExceptionHandler>());
                case ExceptionHandlingPolicy.SendToErrorsQueue:
                    return new ErrorsQueuingExceptionHandler(
                        _queuesFactory.Create<ExceptionDescription>(
                            new QueueCreationOptions
                            {
                                QueueName = $"{queueCreationOptions.QueueName}.Errors",
                                RetriesCount = queueCreationOptions.RetriesCount,
                                RetryInitialTimeout = queueCreationOptions.RetryInitialTimeout,
                                ExceptionHandlingPolicy = ExceptionHandlingPolicy.None
                            }
                        ),
                        _loggerFactory.CreateLogger<ErrorsQueuingExceptionHandler>()
                    );
                default:
                    return new EmptyExceptionHandler(_loggerFactory.CreateLogger<EmptyExceptionHandler>());
            }
        }
    }
}