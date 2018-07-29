namespace Skeleton.Queues.Abstractions.MessagesProcessing
{
    public interface IMessageHandler<in TMessage>
    {
        void Handle(TMessage message);
    }
}