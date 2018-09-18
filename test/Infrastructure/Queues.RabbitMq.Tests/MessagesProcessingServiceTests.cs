namespace Skeleton.Queues.RabbitMq.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Abstractions;
    using Abstractions.QueuesFactory;
    using Abstractions.QueuesFactory.ExceptionsHandling;
    using Abstractions.QueuesFactory.ExceptionsHandling.Handlers;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Handlers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using ProcessingService;
    using QueuesFactory;
    using QueuesFactory.Configuration;
    using Web.Configuration;
    using Web.Testing.Extensions;
    using Xunit;

    public class MessagesProcessingServiceTests
    {
        private readonly Mock<ILogger> _mockLogger;

        private readonly TimeSpan _completionTimeout;
        private readonly CatchingMessageHandler<string> _messageHandler;
        private readonly IHost _host;

        public MessagesProcessingServiceTests()
        {
            _mockLogger = MockLoggerExtensions.CreateMockLogger();

            _messageHandler = new CatchingMessageHandler<string>();
            _host = new HostBuilder()
                .UseEnvironment(EnvironmentName.Development)
                .UseContentRoot(Path.GetDirectoryName(GetType().GetTypeInfo().Assembly.Location))
                .ConfigureAppConfiguration(
                    (context, builder) =>
                    {
                        var hostingEnvironment = context.HostingEnvironment;

                        builder
                            .AddDefaultConfigs(hostingEnvironment.ContentRootPath, hostingEnvironment.EnvironmentName)
                            .AddCiDependentSettings(hostingEnvironment.EnvironmentName);
                    }
                )
                .ConfigureServices(
                    (context, collection) =>
                        collection
                            .AddRabbitMqSupport()
                            .AddNotificationsProcessingService(context.Configuration.GetSection("NotificationsProcessingServiceOptions"))
                            .Configure<TypedRabbitQueuesFactoryOptions>(context.Configuration.GetSection("QueuesFactoryOptions"))
                )
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(
                    builder =>
                    {
                        builder.RegisterInstance(_messageHandler).As<IMessageHandler<string>>();
                    }
                )
                .ConfigureLogging(
                    builder =>
                    {
                        var mockLoggerFactory = new Mock<ILoggerFactory>();
                        mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_mockLogger.Object);
                        builder.Services.Replace(ServiceDescriptor.Singleton(mockLoggerFactory.Object));
                    })
                .Build();
            _completionTimeout = _host.Services.GetService<IConfiguration>().GetSection("CompletionTimeout").Get<TimeSpan>();
        }

        [Fact]
        public async Task ShouldProcessMessageFromQueue()
        {
            // Given
            const string expectedMessage = "Test message";

            // When
            using (var queue = _host.Services.GetService<IGenericQueuesFactory>()
                .Create<string>(_host.Services.GetService<IOptions<NotificationsProcessingServiceOptions>>().Value.QueueCreationOptions)
            )
                await queue.SendMessageAsync(expectedMessage);

            using (_host)
            {
                _host.Start();

                await Task.Delay(_completionTimeout);
                await _host.StopAsync();
            }

            // Then
            _mockLogger.VerifyNoErrorsWasLogged();

            Assert.Equal(expectedMessage, _messageHandler.Messages.Single());
        }
    }
}