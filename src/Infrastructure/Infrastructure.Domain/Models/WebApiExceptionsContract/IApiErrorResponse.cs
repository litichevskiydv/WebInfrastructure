namespace Infrastructure.Domain.Models.WebApiExceptionsContract
{
    public interface IApiErrorResponse<TInnerExceptionData> : IExceptionData<TInnerExceptionData> where TInnerExceptionData : class 
    {
        string Message { get; set; }
    }
}