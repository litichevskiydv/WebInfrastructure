namespace Skeleton.Web.ExceptionsHandling
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using OutputFormatting;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class UnhandledExceptionsLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public UnhandledExceptionsLoggingMiddleware(RequestDelegate next, IHostingEnvironment hostingEnvironment,
            ILogger<UnhandledExceptionsLoggingMiddleware> logger)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            if (hostingEnvironment == null)
                throw new ArgumentNullException(nameof(hostingEnvironment));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _next = next;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;

            _jsonSerializerSettings = new JsonSerializerSettings().UseDefaultSettings();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                context.Response.Clear();
                var httpResponseException = exception as HttpResponseException;

                if (httpResponseException != null)
                    context.Response.StatusCode = (int) httpResponseException.Response.StatusCode;
                else
                {
                    const string message = "Unhandled exception has occurred";
                    _logger.LogError(0, exception, message);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    if (_hostingEnvironment.IsDevelopment() || _hostingEnvironment.IsStaging())
                    {
                        context.Response.ContentType = "application/json; charset=utf-8";
                        using (var output = new StreamWriter(context.Response.Body, Encoding.UTF8, 4096, true))
                            output.WriteLine(JsonConvert.SerializeObject(new ApiErrorResponse(message, exception), _jsonSerializerSettings));
                    }
                }
            }
        }
    }
}