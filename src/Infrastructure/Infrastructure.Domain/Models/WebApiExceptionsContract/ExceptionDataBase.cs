namespace Infrastructure.Domain.Models.WebApiExceptionsContract
{
    public class ExceptionDataBase<TInnerExceptionData> where TInnerExceptionData : class
    {
        public string ExceptionType { get; set; }

        public string ExceptionMessage { get; set; }

        public string StackTrace { get; set; }

        public TInnerExceptionData InnerException { get; set; }
    }
}