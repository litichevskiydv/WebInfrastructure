namespace Skeleton.Queues.Abstractions.QueuesFactory.ExceptionsHandling
{
    public enum ExceptionHandlingPolicy
    {
        None = 0,
        Requeue = 1,
        SendToErrorsQueue = 2,
        Custom = 3
    }
}