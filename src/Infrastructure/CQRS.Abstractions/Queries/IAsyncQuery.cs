namespace Skeleton.CQRS.Abstractions.Queries
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for asynchronously processed queries
    /// </summary>
    /// <typeparam name="TCriterion">Query criterion type</typeparam>
    /// <typeparam name="TResult">Query result type</typeparam>
    public interface IAsyncQuery<in TCriterion, TResult> : IQuery<TCriterion, Task<TResult>> where TCriterion : IAsyncCriterion<TResult>
    {
    }
}