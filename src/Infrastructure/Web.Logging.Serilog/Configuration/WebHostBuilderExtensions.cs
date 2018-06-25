namespace Skeleton.Web.Logging.Serilog.Configuration
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using global::Serilog;
    using global::Serilog.Exceptions;
    using Microsoft.AspNetCore.Hosting;

    public static class WebHostBuilderExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IWebHostBuilder UseSerilog(this IWebHostBuilder hostBuilder)
        {
            if(hostBuilder == null)
                throw new ArgumentNullException(nameof(hostBuilder));

            return UseSerilog(
                hostBuilder,
                configuration => configuration
                    .Enrich.WithApplicationInformationalVersion()
                    .Enrich.WithExceptionDetails()
            );
        }

        [ExcludeFromCodeCoverage]
        public static IWebHostBuilder UseSerilog(
            this IWebHostBuilder hostBuilder, 
            Func<LoggerConfiguration, LoggerConfiguration> loggerConfigurator)
        {
            if (hostBuilder == null)
                throw new ArgumentNullException(nameof(hostBuilder));
            if (loggerConfigurator == null)
                throw new ArgumentNullException(nameof(loggerConfigurator));

            return hostBuilder.UseSerilog(
                (context, configuration) =>
                    loggerConfigurator(configuration)
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext(),
                true
            );
        }
    }
}