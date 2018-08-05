namespace Skeleton.Queues.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMessageHandler<in TMessage>
    {
        Task Handle(TMessage message, CancellationToken cancellationToken);
    }
}