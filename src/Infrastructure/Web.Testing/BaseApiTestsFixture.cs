namespace Skeleton.Web.Testing
{
    using System;
    using System.IO;
    using System.Reflection;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Configuration;
    using Extensions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Moq;

    public abstract class BaseApiTestsFixture : IDisposable
    {
        protected bool Disposed;

        public TestServer Server { get; }
        public Mock<ILogger> MockLogger { get; }

        public IConfigurationRoot Configuration { get; }
        public TimeSpan ApiTimeout { get; }

        protected virtual void OverrideRegisteredDependencies(ContainerBuilder containerBuilder)
        {
        }

        protected BaseApiTestsFixture(Type startupType)
        {
            MockLogger = MockLoggerExtensions.CreateMockLogger();

            var environment = Environment.GetEnvironmentVariable("Hosting:Environment")
                                  ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                                  ?? EnvironmentName.Development;
            var currentDirectory = Path.GetDirectoryName(startupType.GetTypeInfo().Assembly.Location);

            IConfigurationBuilder ConfigurationSetup(IConfigurationBuilder builder, string configsPath, string environmentName) =>
                builder
                    .AddDefaultConfigs(configsPath, environmentName)
                    .AddCiDependentSettings(environmentName);

            Configuration = ConfigurationSetup(new ConfigurationBuilder(), currentDirectory, environment).Build();
            ApiTimeout = Configuration.GetValue<TimeSpan>("ApiTimeout");

            Server = new TestServer(
                new WebHostBuilder()
                    .UseEnvironment(environment)
                    .ConfigureAppConfiguration(builder => ConfigurationSetup(builder, currentDirectory, environment))
                    .ConfigureServices(services => services.AddAutofac())
                    .UseMockLogger(MockLogger)
                    .UseStartup(startupType)
                    .ConfigureTestContainer<ContainerBuilder>(OverrideRegisteredDependencies)
            );
        }

        protected virtual void Dispose(bool disposing)
        {
            if(Disposed)
                return;

            if (disposing)
                Server?.Dispose();
            Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseApiTestsFixture()
        {
            Dispose(false);
        }
    }

    public class BaseApiTestsFixture<TStartup> : BaseApiTestsFixture where TStartup : WebApiBaseStartup
    {
        public BaseApiTestsFixture() : base(typeof(TStartup))
        {
        }
    }
}