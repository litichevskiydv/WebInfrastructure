namespace Skeleton.Web.Logging.ContextsCapturing
{
    using System;
    using System.Linq;
    using CapturedData;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    public static class LoggerExtensions
    {
        public static IDisposable CaptureActionExecutingContext(this ILogger logger, ActionExecutingContext context)
        {
            var capturedContext = new CapturedActionExecutingContext();

            if (context.HttpContext?.Request != null)
            {
                var request = context.HttpContext.Request;

                capturedContext.Method = request.Method;
                capturedContext.Url = request.GetDisplayUrl();
                capturedContext.Headers = request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());
            }
            if (context.ActionArguments != null && context.ActionArguments.Count > 0)
                capturedContext.ActionArguments = context.ActionArguments;

            return logger.BeginScope("{@Context}", capturedContext);
        }
    }
}