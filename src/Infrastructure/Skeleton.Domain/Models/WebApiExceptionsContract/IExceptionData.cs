namespace Skeleton.Domain.Models.WebApiExceptionsContract
{
    public interface IExceptionData<TInnerExceptionData> where TInnerExceptionData : class
    {
        string ExceptionType { get; set; }

        string ExceptionMessage { get; set; }

        string StackTrace { get; set; }

        TInnerExceptionData InnerException { get; set; }
    }
}