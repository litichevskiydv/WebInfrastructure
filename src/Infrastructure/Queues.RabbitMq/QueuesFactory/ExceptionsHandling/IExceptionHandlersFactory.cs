namespace Skeleton.Queues.RabbitMq.QueuesFactory.ExceptionsHandling
{
    using Abstractions.QueuesFactory;
    using Handlers;

    public interface IExceptionHandlersFactory
    {
        ExceptionHandlerBase CreateHandler(QueueCreationOptions queueCreationOptions);
    }
}