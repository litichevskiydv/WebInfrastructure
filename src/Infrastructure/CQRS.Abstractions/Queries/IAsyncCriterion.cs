namespace Skeleton.CQRS.Abstractions.Queries
{
    using System.Threading.Tasks;
    public interface IAsyncCriterion<TResult> : ICriterion<Task<TResult>>
    {
    }
}