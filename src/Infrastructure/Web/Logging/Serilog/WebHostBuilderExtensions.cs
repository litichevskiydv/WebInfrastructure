namespace Skeleton.Web.Logging.Serilog
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using global::Serilog;
    using Microsoft.AspNetCore.Hosting;

    public static class WebHostBuilderExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IWebHostBuilder UseSerilog(
            this IWebHostBuilder hostBuilder,
            Action<LoggerConfiguration> configureLogging
        )
        {
            if(hostBuilder == null)
                throw new ArgumentNullException(nameof(hostBuilder));
            if (configureLogging == null)
                throw new ArgumentNullException(nameof(configureLogging));

            var loggerConfiguration = new LoggerConfiguration();
            hostBuilder.ConfigureServices((context, collection) =>
                                          {
                                              configureLogging(loggerConfiguration);
                                              loggerConfiguration.ReadFrom.Configuration(context.Configuration);

                                              Log.Logger = loggerConfiguration.CreateLogger();
                                          });
            return hostBuilder.UseSerilog();
        }
    }
}