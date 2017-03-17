namespace Skeleton.Web.ExceptionsHandling
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    public class UnhandledExceptionsStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
                   {
                       next(builder.UseUnhandledExceptionsLoggingMiddleware());
                   };
        }
    }
}