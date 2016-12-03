namespace Web.DataAccess.Queries
{
    using System;
    using Domain.Criteria;
    using JetBrains.Annotations;
    using Repositories;
    using Skeleton.CQRS.Abstractions.Queries;

    [UsedImplicitly]
    public class GetValueQuery : IQuery<GetValueQueryCriterion, string>
    {
        private readonly IValuesRepository<string> _repository;

        public GetValueQuery(IValuesRepository<string> repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        public string Ask(GetValueQueryCriterion criterion)
        {
            return _repository.GetOrDefault(criterion.Key);
        }
    }
}