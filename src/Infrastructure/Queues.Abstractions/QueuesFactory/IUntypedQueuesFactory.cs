namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    using Configuration;

    public interface IUntypedQueuesFactory<in TQueueCreationOptions, out TQueue>
        where TQueueCreationOptions : QueueCreationOptions
        where TQueue : QueueBase
    {
        TQueue Create(TQueueCreationOptions creationOptions);
    }
}