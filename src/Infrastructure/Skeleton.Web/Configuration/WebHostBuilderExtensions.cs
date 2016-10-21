namespace Skeleton.Web.Configuration
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseConfigurationFromCommandLine(this IWebHostBuilder webHostBuilder, string[] commandLineArguments)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(commandLineArguments)
                .Build();

            return webHostBuilder.UseConfiguration(configuration);
        }
    }
}