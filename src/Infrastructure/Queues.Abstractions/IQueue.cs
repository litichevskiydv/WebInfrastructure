namespace Skeleton.Queues.Abstractions
{
    public interface IQueue<TMessage>
    {
        void SendMessage(TMessage message);

        void Subscribe(IMessageHandler<TMessage> handler);
    }
}