namespace Infrastructure.Web.ExceptionsHandling
{
    using System;
    using Domain.Models.WebApiExceptionsContract;

    public class ApiErrorResponse : ExceptionData, IHeaderMessage
    {
        public string Message { get; set; }

        public ApiErrorResponse(string message, Exception exception) : base(exception)
        {
            Message = message;
        }
    }
}