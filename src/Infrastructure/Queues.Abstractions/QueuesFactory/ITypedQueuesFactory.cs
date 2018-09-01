namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    using Configuration;

    public interface ITypedQueuesFactory<TQueueCreationOptions> where TQueueCreationOptions : QueueCreationOptions
    {
        ITypedQueue<TMessage> Create<TMessage>(TQueueCreationOptions creationOptions);
    }
}