namespace Skeleton.Web.ExceptionsHandling
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Conventions.Responses;
    using Logging.ContextsCapturing;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    public class UnhandledExceptionsFilterAttribute : ActionFilterAttribute
    {
        private class ExceptionHandlingContext
        {
            public ActionExecutingContext ExecutingContext { get; }
            public ActionExecutedContext ExecutedContext { get; }

            public ExceptionHandlingContext(ActionExecutingContext executingContext, ActionExecutedContext executedContext)
            {
                ExecutingContext = executingContext;
                ExecutedContext = executedContext;
            }
        }

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        private readonly (Type, Action<ExceptionHandlingContext>)[] _handlers;

        private void HandleOperationCanceledException(ExceptionHandlingContext context)
        {
            _logger.LogWarning("Request was cancelled");

            context.ExecutedContext.ExceptionHandled = true;
            context.ExecutedContext.Result = new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        private void HandleException(ExceptionHandlingContext context)
        {
            const string message = "Unhandled exception has occurred";

            using (_logger.CaptureActionExecutingContext(context.ExecutingContext))
                _logger.LogError(context.ExecutedContext.Exception, message);

            context.ExecutedContext.ExceptionHandled = true;
            context.ExecutedContext.Result = _hostingEnvironment.IsDevelopment() || _hostingEnvironment.IsStaging()
                ? new ObjectResult(new ApiExceptionResponse(message, context.ExecutedContext.Exception))
                  {
                      DeclaredType = typeof(ApiExceptionResponse),
                      StatusCode = (int) HttpStatusCode.InternalServerError
                  }
                : new ObjectResult(message) {DeclaredType = typeof(string), StatusCode = (int) HttpStatusCode.InternalServerError};
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
                    (typeof(Exception), new Action<ExceptionHandlingContext>(HandleException))
                };
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionExecutedContext = await next();
            if(actionExecutedContext.Exception == null)
                return;

            var exceptionType = actionExecutedContext.Exception.GetType();
            _handlers
                .FirstOrDefault(x => x.Item1.IsAssignableFrom(exceptionType))
                .Item2(new ExceptionHandlingContext(context, actionExecutedContext));
        }
    }
}