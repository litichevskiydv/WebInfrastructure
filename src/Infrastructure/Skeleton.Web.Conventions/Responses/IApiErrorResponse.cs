namespace Skeleton.Web.Conventions.Responses
{
    public interface IApiErrorResponse<TInnerExceptionData> : IExceptionData<TInnerExceptionData> where TInnerExceptionData : class 
    {
        string Message { get; set; }
    }
}