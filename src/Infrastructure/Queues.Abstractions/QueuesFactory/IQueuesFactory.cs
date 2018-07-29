namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    public interface IQueuesFactory
    {
        IQueue<TMessage> Create<TMessage>(string name);
    }
}