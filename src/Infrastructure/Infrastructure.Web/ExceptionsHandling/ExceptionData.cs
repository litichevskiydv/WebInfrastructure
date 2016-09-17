namespace Infrastructure.Web.ExceptionsHandling
{
    using System;
    using Domain.Models.WebApiExceptionsContract;

    public class ExceptionData : ExceptionDataBase<ExceptionData>
    {
        public ExceptionData(Exception exception)
        {
            ExceptionType = exception.GetType().ToString();
            ExceptionMessage = exception.Message;
            StackTrace = exception.StackTrace;

            if (exception.InnerException != null)
                InnerException = new ExceptionData(exception.InnerException);
        }
    }
}