namespace Skeleton.Queues.Abstractions
{
    public interface IQueuesFactory
    {
        IQueue<TMessage> Create<TMessage>(string name);
    }
}