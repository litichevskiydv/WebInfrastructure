namespace Skeleton.Web.ExceptionsHandling
{
    using System;
    using Conventions.Responses;

    public class ExceptionData : IExceptionData<ExceptionData>
    {
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
        public ExceptionData InnerException { get; set; }

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