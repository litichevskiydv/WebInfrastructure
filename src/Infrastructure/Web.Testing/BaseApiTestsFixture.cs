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
        public TestServer Server { get; }
        public Mock<ILogger> MockLogger { get; }

        public IConfigurationRoot Configuration { get; }
        public int TimeoutInMilliseconds { get; }

        public BaseApiTestsFixture(Type startupType)
        {
            MockLogger = MockLoggerExtensions.CreateMockLogger();

            var environment = Environment.GetEnvironmentVariable("Hosting:Environment")
                              ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                              ?? EnvironmentName.Development;
            var currentDirectory = Path.GetDirectoryName(startupType.GetTypeInfo().Assembly.Location);

            Server = new TestServer(
                         new WebHostBuilder()
                             .UseContentRoot(currentDirectory)
                             .UseEnvironment(environment)
                             .ConfigureServices(services => services.CaptureCommandLineArguments(new string[0]))
                             .UseMockLogger(MockLogger)
                             .UseStartup(startupType));

            Configuration = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("appsettings.json", false, false)
                .AddJsonFile($"appsettings.{environment}.json", true, false)
                .Build();
            TimeoutInMilliseconds = Configuration.GetValue<int>("ApiTimeoutInMilliseconds");
        }

        public void Dispose()
        {
            Server.Dispose();
        }
    }

    public class BaseApiTestsFixture<TStartup> : BaseApiTestsFixture where TStartup : WebApiBaseStartup
    {
        public BaseApiTestsFixture() : base(typeof(TStartup))
        {
        }
    }
}