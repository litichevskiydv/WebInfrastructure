namespace Skeleton.Web.Testing
{
    using System;
    using System.IO;
    using System.Reflection;
    using Autofac;
    using Configuration;
    using Extensions;
    using Hosting;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class BaseApiTestsFixture<TStartup> : WebApplicationFactory<TStartup> where TStartup : WebApiBaseStartup
    {
        private readonly string _environmentName;
        private readonly string _configsDirectory;

        public IConfigurationRoot Configuration { get; }
        public TimeSpan ApiTimeout { get; }

        public Mock<ILogger> MockLogger { get; }

        private static IConfigurationBuilder ConfigurationSetup(IConfigurationBuilder builder, string configsPath, string environmentName) =>
            builder
                .AddDefaultConfigs(configsPath, environmentName)
                .AddCiDependentSettings(environmentName);

        public BaseApiTestsFixture()
        {
            _environmentName
                = Environment.GetEnvironmentVariable("Hosting:Environment")
                  ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                  ?? EnvironmentName.Development;
            _configsDirectory = Path.GetDirectoryName(typeof(TStartup).GetTypeInfo().Assembly.Location);

            Configuration = ConfigurationSetup(new ConfigurationBuilder(), _configsDirectory, _environmentName).Build();
            ApiTimeout = Configuration.GetValue<TimeSpan>("ApiTimeout");

            MockLogger = MockLoggerExtensions.CreateMockLogger();
        }

        protected virtual void OverrideRegisteredDependencies(IServiceCollection serviceCollection)
        {
        }

        protected virtual void OverrideRegisteredDependencies(ContainerBuilder containerBuilder)
        {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseMockLogger(MockLogger)
                .UseEnvironment(_environmentName)
                .ConfigureAppConfiguration(x => ConfigurationSetup(x, _configsDirectory, _environmentName))
                .ConfigureTestServices(OverrideRegisteredDependencies)
                .ConfigureTestContainer<ContainerBuilder>(OverrideRegisteredDependencies);
        }
    }
}