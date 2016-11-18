namespace Skeleton.CQRS.Abstractions.Queries
{
    /// <summary>
    /// Interface for queries
    /// </summary>
    /// <typeparam name="TCriterion">Query criterion type</typeparam>
    /// <typeparam name="TResult">Query result type</typeparam>
    public interface IQuery<in TCriterion, out TResult> where TCriterion : ICriterion
    {
        /// <summary>
        /// Method for query execution
        /// </summary>
        /// <param name="query">Information needed for query execution</param>
        /// <returns>Query result</returns>
        TResult Ask(TCriterion query);
    }
}