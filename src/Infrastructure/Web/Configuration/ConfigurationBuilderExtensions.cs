namespace Skeleton.Web.Configuration
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

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
            IHostingEnvironment hostingEnvironment)
        {
            return configurationBuilder.AddDefaultConfigs(hostingEnvironment.ContentRootPath, hostingEnvironment.EnvironmentName);
        }
    }
}