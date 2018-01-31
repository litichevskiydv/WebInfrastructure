namespace Skeleton.Web.Logging.Serilog
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using global::Serilog;
    using Microsoft.AspNetCore.Hosting;

    public static class WebHostBuilderExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IWebHostBuilder UseSerilog(
            this IWebHostBuilder hostBuilder,
            string versionString
        )
        {
            if(hostBuilder == null)
                throw new ArgumentNullException(nameof(hostBuilder));
            if (string.IsNullOrWhiteSpace(versionString))
                throw new ArgumentNullException(nameof(versionString));

            return hostBuilder.UseSerilog(
                (context, configuration) =>
                    configuration
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("Version", versionString)
                        .ReadFrom.Configuration(context.Configuration),
                true
            );
        }
    }
}