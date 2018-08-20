namespace Skeleton.Queues.RabbitMq.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Abstractions.QueuesFactory;
    using Abstractions.QueuesFactory.ExceptionsHandling;
    using Abstractions.QueuesFactory.ExceptionsHandling.Handlers;
    using Handlers;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using QueuesFactory;
    using QueuesFactory.Configuration;
    using Web.Configuration;
    using Web.Testing.Extensions;
    using Xunit;

    public class RabbitQueueTests
    {
        private readonly TimeSpan _completionTimeout;

        private readonly Mock<ILogger> _mockLogger;
        private readonly IQueuesFactory _queuesFactory;

        public RabbitQueueTests()
        {
            _mockLogger = MockLoggerExtensions.CreateMockLogger();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_mockLogger.Object);
            var loggerFactory = mockLoggerFactory.Object;

            var configuration = new ConfigurationBuilder()
                .AddDefaultConfigs(Path.GetDirectoryName(GetType().GetTypeInfo().Assembly.Location), EnvironmentName.Development)
                .AddCiDependentSettings(EnvironmentName.Development)
                .Build();
            _completionTimeout = configuration.GetSection("CompletionTimeout").Get<TimeSpan>();

            _queuesFactory = new TypedRabbitQueuesFactory(
                new ExceptionHandlersFactory<RabbitMessageDescription>(
                    new ExceptionHandlerBase<RabbitMessageDescription>[]
                    {
                        new EmptyExceptionHandler<RabbitMessageDescription>(
                            loggerFactory.CreateLogger<EmptyExceptionHandler<RabbitMessageDescription>>()
                        ),
                        new RequeuingExceptionHandler<RabbitMessageDescription>(
                            loggerFactory.CreateLogger<RequeuingExceptionHandler<RabbitMessageDescription>>()
                        ),
                        new ErrorsQueuingExceptionHandler<RabbitMessageDescription>(
                            loggerFactory.CreateLogger<ErrorsQueuingExceptionHandler<RabbitMessageDescription>>()
                        )
                    }
                ),
                loggerFactory,
                Options.Create(configuration.GetSection("QueuesFactoryOptions").Get<TypedRabbitQueuesFactoryOptions>())
            );
        }

        [Fact]
        public async Task ShouldProcessMessageFromQueue()
        {
            // Given
            const string expectedMessage = "Test message";
            var queueCreationOptions
                = new RabbitQueueCreationOptions
                  {
                      QueueName = Guid.NewGuid().ToString(),
                      RetriesCount = 0,
                      RetryInitialTimeout = TimeSpan.Zero,
                      ExceptionHandlingPolicy = ExceptionHandlingPolicy.None
                  };


            // When
            var messageHandler = new CatchingMessageHandler();

            using (var queue = _queuesFactory.Create<string>(queueCreationOptions))
            {
                await queue.SendMessageAsync(expectedMessage);
                await queue.SubscribeAsync(messageHandler);

                await Task.Delay(_completionTimeout);
            }

            // Then
            _mockLogger.VerifyNoErrorsWasLogged();

            Assert.Equal(expectedMessage, messageHandler.Messages.Single());
        }
    }
}