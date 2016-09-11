namespace Infrastructure.Web.ExceptionsHandling
{
    using System;
    using JetBrains.Annotations;

    public class ExceptionData
    {
        public string ExceptionType { [UsedImplicitly] get; set; }

        public string ExceptionMessage { [UsedImplicitly] get; set; }

        public string StackTrace { [UsedImplicitly] get; set; }

        public ExceptionData InnerException { [UsedImplicitly] get; set; }

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