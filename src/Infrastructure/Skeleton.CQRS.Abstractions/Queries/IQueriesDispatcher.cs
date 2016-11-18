namespace Skeleton.CQRS.Abstractions.Queries
{
    /// <summary>
    /// Queries dispatcher interface
    /// </summary>
    public interface IQueriesDispatcher
    {
        /// <summary>
        /// Method for queries execution
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="query">Information needed for queries execution</param>
        /// <returns>Query result</returns>
        TResult Execute<TResult>(ICriterion query);
    }
}