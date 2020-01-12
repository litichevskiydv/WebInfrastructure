namespace Skeleton.Web.Configuration
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddDefaultConfigs(
            this IConfigurationBuilder configurationBuilder, 
            string configsPath,
            string environmentName)
        {
            return configurationBuilder
                .AddEnvironmentVariables()
                .SetBasePath(configsPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environmentName}.json", false, true);
        }

        public static IConfigurationBuilder AddDefaultConfigs(
            this IConfigurationBuilder configurationBuilder,
            IHostEnvironment hostEnvironment)
        {
            return configurationBuilder.AddDefaultConfigs(hostEnvironment.ContentRootPath, hostEnvironment.EnvironmentName);
        }
    }
}