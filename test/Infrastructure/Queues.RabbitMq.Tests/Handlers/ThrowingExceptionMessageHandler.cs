namespace Skeleton.Queues.RabbitMq.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;

    public class ThrowingExceptionMessageHandler : IMessageHandler<string>
    {
        private int _counter;
        private readonly List<string> _messages;

        public IReadOnlyCollection<string> Messages => _messages;

        public ThrowingExceptionMessageHandler()
        {
            _counter = 0;
            _messages = new List<string>();
        }

        public Task Handle(string message, CancellationToken cancellationToken)
        {
            _counter++;
            if(_counter == 1)
                throw new InvalidOperationException();

            _messages.Add(message);
            return Task.CompletedTask;
        }
    }
}