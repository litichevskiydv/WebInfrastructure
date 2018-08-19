namespace Skeleton.Queues.RabbitMq.Tests.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;

    public class CatchingMessageHandler : IMessageHandler<string>
    {
        private readonly List<string> _messages = new List<string>();

        public IReadOnlyCollection<string> Messages => _messages;

        public Task Handle(string message, CancellationToken cancellationToken)
        {
            _messages.Add(message);
            return Task.CompletedTask;
        }
    }
}