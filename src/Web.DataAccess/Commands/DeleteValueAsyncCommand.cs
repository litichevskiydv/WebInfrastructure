namespace Web.DataAccess.Commands
{
    using System;
    using System.Threading.Tasks;
    using Domain.CommandContexts;
    using JetBrains.Annotations;
    using Repositories;
    using Skeleton.CQRS.Abstractions.Commands;

    [UsedImplicitly]
    public class DeleteValueAsyncCommand : IAsyncCommand<DeleteValueAsyncCommandContext>
    {
        private readonly IValuesRepository<string> _repository;

        public DeleteValueAsyncCommand(IValuesRepository<string> repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        public Task Execute(DeleteValueAsyncCommandContext commandContext)
        {
            _repository.Delete(commandContext.Key);
            return Task.FromResult(0);
        }
    }
}