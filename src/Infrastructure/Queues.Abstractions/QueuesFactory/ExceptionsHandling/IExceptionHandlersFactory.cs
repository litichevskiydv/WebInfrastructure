namespace Skeleton.Queues.Abstractions.QueuesFactory.ExceptionsHandling
{
    using Handlers;
    using QueuesFactory;

    public interface IExceptionHandlersFactory<TMessageDescription> where TMessageDescription : QueueMessageDescriptionBase, new()
    {
        ExceptionHandlerBase<TMessageDescription> GetHandler(ExceptionHandlingPolicy policy);
    }
}