namespace Skeleton.Web.ExceptionsHandling
{
    using System;
    using System.Net;
    using System.Web.Http;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    public class UnhandledExceptionsFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        public UnhandledExceptionsFilterAttribute(IHostingEnvironment hostingEnvironment, ILogger<UnhandledExceptionsFilterAttribute> logger)
        {
            if (hostingEnvironment == null)
                throw new ArgumentNullException(nameof(hostingEnvironment));
            if(logger == null)
                throw new ArgumentNullException(nameof(logger));

            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var httpResponseException = context.Exception as HttpResponseException;
            if (httpResponseException != null)
                context.Result = new ResponseMessageResult(httpResponseException.Response)
                                 {
                                     StatusCode = (int) httpResponseException.Response.StatusCode
                                 };
            else
            {
                const string message = "Unhandled exception has occurred";
                _logger.LogError(0, context.Exception, message);

                context.Result = _hostingEnvironment.IsDevelopment() || _hostingEnvironment.IsStaging()
                    ? new ObjectResult(new ApiErrorResponse(message, context.Exception))
                      {
                          DeclaredType = typeof(ApiErrorResponse),
                          StatusCode = (int) HttpStatusCode.InternalServerError
                      }
                    : new ObjectResult(message) {DeclaredType = typeof(string), StatusCode = (int) HttpStatusCode.InternalServerError};
            }
        }
    }
}