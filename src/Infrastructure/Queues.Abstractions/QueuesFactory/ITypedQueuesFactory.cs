namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    public interface ITypedQueuesFactory<TQueue> : IQueuesFactory where TQueue : QueueBase
    {
    }
}