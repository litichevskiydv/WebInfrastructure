namespace Skeleton.Web.Testing.Extensions
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationBuilderExtensions
    {
        private static readonly string[] KnownCiNames;

        static ConfigurationBuilderExtensions()
        {
            KnownCiNames = new[]
                            {
                                "Appveyor",
                                "Travis"
                            };
        }

        public static IConfigurationBuilder AddCiDependentSettings(this IConfigurationBuilder configurationBuilder, string environment)
        {
            var ciName = KnownCiNames.FirstOrDefault(x => Environment.GetEnvironmentVariable(x.ToUpper())?.ToUpperInvariant() == "TRUE");
            configurationBuilder.AddJsonFile($"appsettings.{environment}.{ciName ?? ""}.json", true, false);

            if(Environment.GetEnvironmentVariable("CI_WINDOWS")?.ToUpperInvariant() == "TRUE")
                configurationBuilder.AddJsonFile($"appsettings.{environment}.{ciName ?? ""}.Windows.json", true, false);

            if (Environment.GetEnvironmentVariable("CI_LINUX")?.ToUpperInvariant() == "TRUE")
                configurationBuilder.AddJsonFile($"appsettings.{environment}.{ciName ?? ""}.Linux.json", true, false);

            return configurationBuilder;
        }
    }
}