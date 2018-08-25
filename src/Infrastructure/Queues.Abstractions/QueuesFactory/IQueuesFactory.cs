namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    using Configuration;

    public interface IQueuesFactory
    {
        ITypedQueue<TMessage> Create<TMessage>(QueueCreationOptions creationOptions);
    }
}