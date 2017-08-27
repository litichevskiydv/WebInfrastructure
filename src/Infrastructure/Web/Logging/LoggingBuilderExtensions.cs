namespace Skeleton.Web.Logging
{
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using NLog;
    using NLog.Extensions.Logging;

    public static class LoggerFactoryExtensions
    {
        [ExcludeFromCodeCoverage]
        public static ILoggingBuilder AddNLog(this ILoggingBuilder loggingBuilder)
        {
            LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging")));
            LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Abstractions")));
            LogManager.AddHiddenAssembly(typeof(ConfigureExtensions).GetTypeInfo().Assembly);
            LogManager.AddHiddenAssembly(typeof(LoggerFactoryExtensions).GetTypeInfo().Assembly);

            loggingBuilder.AddProvider(new NLogLoggerProvider());
            return loggingBuilder;
        }
    }
}
