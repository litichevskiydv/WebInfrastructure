namespace Skeleton.Queues.Abstractions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using QueuesFactory;

    public abstract class MessagesProcessingService<TMessage> : IHostedService, IDisposable
    {
        protected readonly IGenericQueuesFactory QueuesFactory;
        protected readonly IMessageHandler<TMessage> MessageHandler;
        protected readonly ILogger Logger;
        protected readonly MessagesProcessingServiceOptions Options;
        
        protected readonly CancellationTokenSource StoppingCts;
        protected ITypedQueue<TMessage> Queue;
        protected bool Disposed;

        protected MessagesProcessingService(
            IGenericQueuesFactory queuesFactory, 
            IMessageHandler<TMessage> messageHandler, 
            ILogger logger, 
            MessagesProcessingServiceOptions options)
        {
            if(queuesFactory == null)
                throw new ArgumentNullException(nameof(queuesFactory));
            if(messageHandler == null)
                throw new ArgumentNullException(nameof(messageHandler));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            QueuesFactory = queuesFactory;
            MessageHandler = messageHandler;
            Logger = logger;
            Options = options;

            StoppingCts = new CancellationTokenSource();
        }

        private async Task EstablishConnection(CancellationToken stoppingToken)
        {
            while (Queue == null && stoppingToken.IsCancellationRequested == false)
            {
                try
                {
                    Queue = await QueuesFactory.Create<TMessage>(Options.QueueCreationOptions)
                        .SubscribeAsync(MessageHandler, StoppingCts.Token);
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "Error has been occurred during establishing connection to queue");
                    await Task.Delay(Options.ConnectionAttemptTimeout, stoppingToken);
                }
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Starting service...");
            await EstablishConnection(cancellationToken);
            Logger.LogInformation("Service was started");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                StoppingCts.Cancel();
                Queue?.Dispose();
            }

            Disposed = true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Stopping service...");
            Dispose(true);
            Logger.LogInformation("Service was stopped");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MessagesProcessingService()
        {
            Dispose(false);
        }
    }
}