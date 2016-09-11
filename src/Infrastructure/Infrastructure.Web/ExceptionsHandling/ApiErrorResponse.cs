namespace Infrastructure.Web.ExceptionsHandling
{
    using System;
    using JetBrains.Annotations;

    public class ApiErrorResponse : ExceptionData
    {
        public string Message { [UsedImplicitly] get; set; }

        public ApiErrorResponse(string message, Exception exception) : base(exception)
        {
            Message = message;
        }
    }
}