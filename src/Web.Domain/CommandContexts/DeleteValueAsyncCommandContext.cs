namespace Web.Domain.CommandContexts
{
    using Skeleton.CQRS.Abstractions.Commands;

    public class DeleteValueAsyncCommandContext : ICommandContext
    {
        public int Key { get; }

        public DeleteValueAsyncCommandContext(int key)
        {
            Key = key;
        }
    }
}