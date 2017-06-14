namespace Skeleton.CQRS.Abstractions.Queries
{
    using System.Threading.Tasks;

    /// <summary>
    /// Extensions for queries dispatcher
    /// </summary>
    public static class QueriesDispatcherExtensions
    {
        /// <summary>
        /// Method for asynchronous queries execution
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="queriesDispatcher">Queries dispatcher</param>
        /// <param name="criterion">Information needed for queries execution</param>
        /// <returns>Task for asynchronous operation</returns>
        public static Task<TResult> ExecuteAsync<TResult>(this IQueriesDispatcher queriesDispatcher, IAsyncCriterion<TResult> criterion)
        {
            return queriesDispatcher.Execute(criterion);
        }
    }
}