namespace Web.DataAccess.Queries
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Criteria;
    using Domain.Dtos;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Options;
    using Repositories;
    using Skeleton.CQRS.Abstractions.Queries;

    [UsedImplicitly]
    public class GetAllValuesQuery : IAsyncQuery<GetAllValuesQueryCriterion, string[]>
    {
        private readonly IValuesRepository<string> _repository;
        private readonly DefaultConfigurationValues _defaultConfigurationValues;

        public GetAllValuesQuery(IValuesRepository<string> repository, IOptions<DefaultConfigurationValues> defaultConfigurationValues)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            if (defaultConfigurationValues == null)
                throw new ArgumentNullException(nameof(defaultConfigurationValues));

            _repository = repository;
            _defaultConfigurationValues = defaultConfigurationValues.Value;
        }

        public Task<string[]> Ask(GetAllValuesQueryCriterion criterion)
        {
            var values = _repository.Get();
            var valuesArray = values as string[] ?? values.ToArray();

            return Task.FromResult(valuesArray.Length == 0 ? _defaultConfigurationValues.Values : valuesArray);
        }
    }
}