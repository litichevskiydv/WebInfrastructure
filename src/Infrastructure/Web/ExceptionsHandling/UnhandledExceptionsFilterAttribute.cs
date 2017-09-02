namespace Skeleton.Web.ExceptionsHandling
{
    using System;
    using System.Linq;
    using System.Net;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    public class UnhandledExceptionsFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        private readonly (Type, Action<ExceptionContext>)[] _handlers;

        private void HandleOperationCanceledException(ExceptionContext context)
        {
            _logger.LogWarning("Request was cancelled");

            context.ExceptionHandled = true;
            context.Result = new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        private void HandleException(ExceptionContext context)
        {
            const string message = "Unhandled exception has occurred";
            _logger.LogError(context.Exception, message);

            context.Result = _hostingEnvironment.IsDevelopment() || _hostingEnvironment.IsStaging()
                ? new ObjectResult(new ApiErrorResponse(message, context.Exception))
                  {
                      DeclaredType = typeof(ApiErrorResponse),
                      StatusCode = (int)HttpStatusCode.InternalServerError
                  }
                : new ObjectResult(message) { DeclaredType = typeof(string), StatusCode = (int)HttpStatusCode.InternalServerError };
        }

        public UnhandledExceptionsFilterAttribute(IHostingEnvironment hostingEnvironment, ILogger<UnhandledExceptionsFilterAttribute> logger)
        {
            if (hostingEnvironment == null)
                throw new ArgumentNullException(nameof(hostingEnvironment));
            if(logger == null)
                throw new ArgumentNullException(nameof(logger));

            _hostingEnvironment = hostingEnvironment;
            _logger = logger;

            _handlers =
                new[]
                {
                    (typeof(OperationCanceledException), HandleOperationCanceledException),
                    (typeof(Exception), new Action<ExceptionContext>(HandleException))
                };
        }

        public override void OnException(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType();
            _handlers.FirstOrDefault(x => x.Item1.IsAssignableFrom(exceptionType)).Item2(context);
        }
    }
}