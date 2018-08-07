namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    using System;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public abstract class QueueBase : IDisposable
    {
        protected delegate Task ActionForAcknowledge();
        protected delegate Task ActionForExceptionHandling(
            Exception exception,
            string messageId,
            string messageContent,
            CancellationToken cancellationToken);
        protected delegate Task ActionForMessageHandling(
            string messageId, 
            string messageContent, 
            ActionForAcknowledge actionForAcknowledge,
            ActionForExceptionHandling actionForExceptionHandling);

        private readonly int _retriesCount;
        private readonly TimeSpan _retryInitialTimeout;
        protected readonly ILogger Logger;

        protected bool Disposed;

        protected QueueBase(
            int retriesCount,
            TimeSpan retryInitialTimeout,
            ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _retriesCount = retriesCount;
            _retryInitialTimeout = retryInitialTimeout;

            Logger = logger;
        }

        protected async Task RetryAsync(Func<Task> action, CancellationToken cancellationToken)
        {
            var succeeded = false;
            ExceptionDispatchInfo lastExceptionDispatchInfo = null;

            for (var i = 0; i < _retriesCount && cancellationToken.IsCancellationRequested == false; i++)
            {
                try
                {
                    await action();

                    succeeded = true;
                    break;
                }
                catch (Exception e)
                {
                    lastExceptionDispatchInfo = ExceptionDispatchInfo.Capture(e);
                }

                if (i != _retriesCount - 1)
                    await Task.Delay(_retryInitialTimeout, cancellationToken);
            }

            if(succeeded == false)
                lastExceptionDispatchInfo?.Throw();
        }

        protected abstract Task SendMessageAsync(string content);

        protected async Task SendMessageAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        {
            try
            {
                var content = JsonConvert.SerializeObject(message);
                await RetryAsync(() => SendMessageAsync(content), cancellationToken);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error has been occurred during sending the message");
            }
        }

        protected abstract void Subscribe(ActionForMessageHandling actionForMessageHandling);

        protected void Subscribe<TMessage>(IMessageHandler<TMessage> handler, CancellationToken cancellationToken)
        {
            Subscribe(
                async (messageId, messageContent, actionForAcknowledge, actionForExceptionHandling) =>
                {
                    try
                    {
                        var message = JsonConvert.DeserializeObject<TMessage>(messageContent);
                        await RetryAsync(() => handler.Handle(message, cancellationToken), cancellationToken);
                        await RetryAsync(() => actionForAcknowledge(), cancellationToken);
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(e, $"Error has been occurred during processing the message, Id {messageId}");
                        await actionForExceptionHandling(e, messageId, messageContent, cancellationToken);
                    }
                }
            );
        }

        protected abstract void Dispose(bool disposing);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~QueueBase()
        {
            Dispose(false);
        }
    }
}