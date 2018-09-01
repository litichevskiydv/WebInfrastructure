namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    using Configuration;

    public interface IGenericQueuesFactory
    {
        ITypedQueue<TMessage> Create<TMessage>(QueueCreationOptions creationOptions);
    }
}