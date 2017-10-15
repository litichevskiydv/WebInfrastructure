namespace Skeleton.Web.Logging.NLog
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using global::NLog;
    using global::NLog.Config;
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationBuilderExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IConfigurationBuilder AddNLogConfig(this IConfigurationBuilder configurationBuilder, string relativeConfigPath)
        {
            var fullConfigPath = Path.Combine(configurationBuilder.GetFileProvider().GetFileInfo(relativeConfigPath).PhysicalPath);
            LogManager.Configuration = new XmlLoggingConfiguration(fullConfigPath, false);
            return configurationBuilder;
        }
    }
}