namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ITypedQueue<TMessage> : IDisposable
    {
        Task<ITypedQueue<TMessage>> SendMessageAsync(TMessage message, CancellationToken cancellationToken = default(CancellationToken));

        Task<ITypedQueue<TMessage>> SubscribeAsync(IMessageHandler<TMessage> handler, CancellationToken cancellationToken = default(CancellationToken));
    }
}