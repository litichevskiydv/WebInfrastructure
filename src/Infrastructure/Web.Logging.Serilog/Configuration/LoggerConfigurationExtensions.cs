namespace Skeleton.Web.Logging.Serilog.Configuration
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Enrichers;
    using global::Serilog;
    using global::Serilog.Exceptions;
    using Microsoft.Extensions.Configuration;

    public static class LoggerConfigurationExtensions
    {
        [ExcludeFromCodeCoverage]
        public static LoggerConfiguration UseDefaultSettings(
            this LoggerConfiguration loggerConfiguration,
            IConfiguration configuration)
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            return loggerConfiguration
                .Enrich.WithApplicationInformationalVersion()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMessageTemplateHash()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration);
        }
    }
}