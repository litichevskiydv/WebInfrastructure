namespace Skeleton.CQRS.Implementations.Queries.QueriesFactory
{
    using System;
    using Abstractions.Queries;

    /// <summary>
    /// Queries factory based on IServiceProvider
    /// </summary>
    public class GenericQueriesFactory : IQueriesFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericQueriesFactory(IServiceProvider serviceProvider)
        {
            if(serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public IQuery<TCriterion, TResult> Create<TCriterion, TResult>() where TCriterion : ICriterion<TResult>
        {
            return (IQuery<TCriterion, TResult>)_serviceProvider.GetService(typeof(IQuery<TCriterion, TResult>));
        }
    }
}