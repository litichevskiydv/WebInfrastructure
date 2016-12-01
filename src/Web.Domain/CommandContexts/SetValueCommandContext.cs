namespace Web.Domain.CommandContexts
{
    using Skeleton.CQRS.Abstractions.Commands;
    public class SetValueCommandContext : ICommandContext
    {
        public int Key { get; }
        public string Value { get; }

        public SetValueCommandContext(int key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}