namespace Web.DataAccess.Commands
{
    using System;
    using Domain.CommandContexts;
    using JetBrains.Annotations;
    using Repositories;
    using Skeleton.CQRS.Abstractions.Commands;

    [UsedImplicitly]
    public class SetValueCommand : ICommand<SetValueCommandContext>
    {
        private readonly IValuesRepository<string> _repository;

        public SetValueCommand(IValuesRepository<string> repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        public void Execute(SetValueCommandContext commandContext)
        {
            _repository.Set(commandContext.Key, commandContext.Value);
        }
    }
}