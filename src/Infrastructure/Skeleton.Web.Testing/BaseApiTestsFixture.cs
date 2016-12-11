namespace Skeleton.Web.Testing
{
    using System;
    using System.IO;
    using Configuration;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Moq;

    public abstract class BaseApiTestsFixture : IDisposable
    {
        public TestServer Server { get; }
        public Mock<ILogger> Logger { get; }

        public IConfigurationRoot Configuration { get; }
        public int TimeoutInMilliseconds { get; }

        public BaseApiTestsFixture(Type startupType)
        {
            Logger = new Mock<ILogger>();
            Logger.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(Logger.Object);

            var environment = Environment.GetEnvironmentVariable("Hosting:Environment")
                              ?? Environment.GetEnvironmentVariable("ASPNET_ENV")
                              ?? "Development";
            var currentDirectory = Directory.GetCurrentDirectory();

            Server = new TestServer(
                         new WebHostBuilder()
                             .UseContentRoot(currentDirectory)
                             .UseEnvironment(environment)
                             .ConfigureServices(services => services.CaptureCommandLineArguments(new string[0]))
                             .UseLoggerFactory(mockLoggerFactory.Object)
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