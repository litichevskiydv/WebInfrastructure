namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    using System;
    using MessagesProcessing;

    public interface IQueue<TMessage> : IDisposable
    {
        IQueue<TMessage> SendMessage(TMessage message);

        IQueue<TMessage> Subscribe(IMessageHandler<TMessage> handler);
    }
}