namespace Skeleton.Web.Logging.NLog
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using global::NLog;
    using global::NLog.Extensions.Logging;
    using Microsoft.Extensions.Logging;

    public static class LoggingBuilderExtensions
    {
        [ExcludeFromCodeCoverage]
        public static ILoggingBuilder AddNLog(this ILoggingBuilder loggingBuilder)
        {
            LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging")));
            LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Abstractions")));
            LogManager.AddHiddenAssembly(typeof(ConfigureExtensions).GetTypeInfo().Assembly);
            LogManager.AddHiddenAssembly(typeof(LoggingBuilderExtensions).GetTypeInfo().Assembly);

            loggingBuilder.AddProvider(new NLogLoggerProvider());
            return loggingBuilder;
        }
    }
}
