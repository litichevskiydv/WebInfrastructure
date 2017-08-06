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
            return configurationBuilder
                .AddJsonFile($"appsettings.{environment}.{ciName ?? ""}.json", true, false);
        }
    }
}