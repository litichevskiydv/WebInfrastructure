namespace Infrastructure.Web.ExceptionsHandling
{
    using System;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    public class UnhandledExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public UnhandledExceptionFilterAttribute(ILogger<UnhandledExceptionFilterAttribute> logger)
        {
            if(logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(0, context.Exception, "Unhandled exception has occurred");
        }
    }
}