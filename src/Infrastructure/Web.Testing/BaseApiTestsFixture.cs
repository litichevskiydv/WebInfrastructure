namespace Skeleton.Web.Testing
{
    using System;
    using System.IO;
    using System.Reflection;
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
        public int TimeoutInMilliseconds { get; }

        protected BaseApiTestsFixture(Type startupType)
        {
            MockLogger = MockLoggerExtensions.CreateMockLogger();

            var environment = Environment.GetEnvironmentVariable("Hosting:Environment")
                                  ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                                  ?? EnvironmentName.Development;
            var currentDirectory = Path.GetDirectoryName(startupType.GetTypeInfo().Assembly.Location);

            Func<IConfigurationBuilder, string, string, IConfigurationBuilder> configurationSetup =
                (builder, configsPath, environmentName) =>
                    builder
                        .AddDefaultConfigs(configsPath, environmentName)
                        .AddCiDependentSettings(environmentName);
            Configuration = configurationSetup(new ConfigurationBuilder(), currentDirectory, environment).Build();
            TimeoutInMilliseconds = Configuration.GetValue<int>("ApiTimeoutInMilliseconds");

            Server = new TestServer(
                new WebHostBuilder()
                    .UseEnvironment(environment)
                    .ConfigureAppConfiguration(builder => configurationSetup(builder, currentDirectory, environment))
                    .UseMockLogger(MockLogger)
                    .UseStartup(startupType));
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