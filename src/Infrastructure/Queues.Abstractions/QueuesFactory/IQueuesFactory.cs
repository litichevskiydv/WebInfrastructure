namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    public interface IQueuesFactory
    {
        ITypedQueue<TMessage> Create<TMessage>(QueueCreationOptions creationOptions);
    }
}