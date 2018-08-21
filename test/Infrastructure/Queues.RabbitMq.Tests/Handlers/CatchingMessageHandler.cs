namespace Skeleton.Queues.RabbitMq.Tests.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;

    public class CatchingMessageHandler<TMessage> : IMessageHandler<TMessage>
    {
        private readonly List<TMessage> _messages = new List<TMessage>();

        public IReadOnlyCollection<TMessage> Messages => _messages;

        public Task Handle(TMessage message, CancellationToken cancellationToken)
        {
            _messages.Add(message);
            return Task.CompletedTask;
        }
    }
}