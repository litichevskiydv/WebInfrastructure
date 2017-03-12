namespace Skeleton.Web.ExceptionsHandling
{
    using System;
    using Conventions.Responses;

    public class ApiErrorResponse : ExceptionData, IApiErrorResponse<ExceptionData>
    {
        public string Message { get; set; }

        public ApiErrorResponse(string message, Exception exception) : base(exception)
        {
            Message = message;
        }
    }
}