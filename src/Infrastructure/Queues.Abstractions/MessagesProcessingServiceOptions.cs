namespace Skeleton.Queues.Abstractions
{
    using System;
    using QueuesFactory.Configuration;

    public abstract class MessagesProcessingServiceOptions
    {
        public TimeSpan ConnectionAttemptTimeout { get; set; }

        public QueueCreationOptions QueueCreationOptions { get; set; }

        internal MessagesProcessingServiceOptions()
        {
        }
    }

    public abstract class MessagesProcessingServiceOptions<TQueueCreationOptions> : MessagesProcessingServiceOptions
        where TQueueCreationOptions : QueueCreationOptions
    {
        public new TQueueCreationOptions QueueCreationOptions
        {
            get => (TQueueCreationOptions)base.QueueCreationOptions;
            set => base.QueueCreationOptions = value;
        }
    }
}