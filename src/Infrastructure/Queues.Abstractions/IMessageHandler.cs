namespace Skeleton.Queues.Abstractions
{
    public interface IMessageHandler<in TMessage>
    {
        void Handle(TMessage message);
    }
}