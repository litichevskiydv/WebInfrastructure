namespace Skeleton.Web.Testing.Extensions
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
    using Moq;

    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseMockLogger(this IWebHostBuilder builder, Mock<ILogger> mockLogger)
        {
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(mockLogger.Object);

            return builder.ConfigureLogging(x => x.Services.Replace(ServiceDescriptor.Singleton(mockLoggerFactory.Object)));
        }
    }
}