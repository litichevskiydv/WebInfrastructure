namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    using Configuration;

    public abstract class TypedQueuesFactory<TMessageDescription, TQueueCreationOptions> : IQueuesFactory
        where TMessageDescription : QueueMessageDescriptionBase, new()
        where TQueueCreationOptions : QueueCreationOptions<TMessageDescription>, new()
    {
        protected abstract ITypedQueue<TMessage> Create<TMessage>(TQueueCreationOptions creationOptions);

        public ITypedQueue<TMessage> Create<TMessage>(QueueCreationOptions creationOptions)
        {
            return Create<TMessage>((TQueueCreationOptions) creationOptions);
        }
    }
}