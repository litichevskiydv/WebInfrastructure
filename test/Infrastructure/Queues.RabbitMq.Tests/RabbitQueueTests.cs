namespace Skeleton.Queues.RabbitMq.Tests
{
    using System;
    using System.Collections.Generic;
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
    using QueuesFactory.ExceptionsHandling.Handlers;
    using RabbitMQ.Client;
    using Web.Configuration;
    using Web.Testing.Extensions;
    using Xunit;

    public class RabbitQueueTests
    {
        private class QueuesFactoryForTests : RabbitQueuesFactory
        {
            private readonly IConnection _connection;
            private readonly ITypedQueue<ExceptionDescription> _errorsQueue;

            public QueuesFactoryForTests(
                IExceptionHandlersFactory<RabbitMessageDescription> exceptionHandlersFactory,
                ILoggerFactory loggerFactory,
                IOptions<TypedRabbitQueuesFactoryOptions> options, 
                IConnection connection, 
                ITypedQueue<ExceptionDescription> errorsQueue)
                : base(exceptionHandlersFactory, loggerFactory, options)
            {
                _connection = connection;
                _errorsQueue = errorsQueue;
            }

            protected override IConnection CreateConnection(string[] hosts)
            {
                return _connection;
            }

            protected override ITypedQueue<ExceptionDescription> CreateErrorsQueue(RabbitQueueCreationOptions parentQueueCreationOptions)
            {
                return _errorsQueue;
            }
        }

        
        private readonly Mock<ILogger> _mockLogger;
        private readonly ILoggerFactory _loggerFactory;

        private readonly TimeSpan _completionTimeout;
        private readonly RabbitQueuesFactory _queuesFactory;
        private readonly IConnectionFactory _connectionsFactory;

        public RabbitQueueTests()
        {
            _mockLogger = MockLoggerExtensions.CreateMockLogger();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_mockLogger.Object);
            _loggerFactory = mockLoggerFactory.Object;

            var configuration = new ConfigurationBuilder()
                .AddDefaultConfigs(Path.GetDirectoryName(GetType().GetTypeInfo().Assembly.Location), EnvironmentName.Development)
                .AddCiDependentSettings(EnvironmentName.Development)
                .Build();
            _completionTimeout = configuration.GetSection("CompletionTimeout").Get<TimeSpan>();
            var queuesFactoryOptions = configuration.GetSection("QueuesFactoryOptions").Get<TypedRabbitQueuesFactoryOptions>();

            _connectionsFactory
                = new ConnectionFactory
                  {
                      UserName = queuesFactoryOptions.Credentials.UserName,
                      Password = queuesFactoryOptions.Credentials.Password,
                      AutomaticRecoveryEnabled = true,
                      NetworkRecoveryInterval = queuesFactoryOptions.NetworkRecoveryInterval,
                      TopologyRecoveryEnabled = true,
                      HostName = queuesFactoryOptions.Hosts.Single()
                };
            _queuesFactory = new RabbitQueuesFactory(
                new ExceptionHandlersFactory<RabbitMessageDescription>(
                    new ExceptionHandlerBase<RabbitMessageDescription>[]
                    {
                        new EmptyExceptionHandler<RabbitMessageDescription>(
                            _loggerFactory.CreateLogger<EmptyExceptionHandler<RabbitMessageDescription>>()
                        ),
                        new RequeuingExceptionHandler<RabbitMessageDescription>(
                            _loggerFactory.CreateLogger<RequeuingExceptionHandler<RabbitMessageDescription>>()
                        ),
                        new ErrorsQueuingExceptionHandler<RabbitMessageDescription>(
                            _loggerFactory.CreateLogger<ErrorsQueuingExceptionHandler<RabbitMessageDescription>>()
                        )
                    }
                ),
                _loggerFactory,
                Options.Create(queuesFactoryOptions)
            );
        }

        private ulong GetQueueMessagesCount(string queueName)
        {
            using (var conection = _connectionsFactory.CreateConnection())
            using (var queue = conection.CreateModel())
                return queue.MessageCount(queueName);
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
            var messageHandler = new CatchingMessageHandler<string>();

            using (var queue = _queuesFactory.Create<string>(queueCreationOptions))
            {
                await queue.SendMessageAsync(expectedMessage);
                await queue.SubscribeAsync(messageHandler);

                await Task.Delay(_completionTimeout);
            }

            // Then
            _mockLogger.VerifyNoErrorsWasLogged();

            Assert.Equal(expectedMessage, messageHandler.Messages.Single());
            Assert.Equal(0UL, GetQueueMessagesCount(queueCreationOptions.QueueName));
        }

        [Fact]
        public async Task ShouldDeleteBadMessageFromQueue()
        {
            // Given
            var queueCreationOptions
                = new RabbitQueueCreationOptions
                  {
                      QueueName = Guid.NewGuid().ToString(),
                      RetriesCount = 0,
                      RetryInitialTimeout = TimeSpan.Zero,
                      ExceptionHandlingPolicy = ExceptionHandlingPolicy.None
                  };


            // When
            var messageHandler = new ThrowingExceptionMessageHandler();

            using (var queue = _queuesFactory.Create<string>(queueCreationOptions))
            {
                await queue.SendMessageAsync("Test message");
                await queue.SubscribeAsync(messageHandler);

                await Task.Delay(_completionTimeout);
            }

            // Then
            _mockLogger.VerifyErrorWasLogged<InvalidOperationException>();

            Assert.Empty(messageHandler.Messages);
            Assert.Equal(0UL, GetQueueMessagesCount(queueCreationOptions.QueueName));
        }

        [Fact]
        public async Task ShouldProcessMessageOnSecondAttempt()
        {
            // Given
            const string expectedMessage = "Test message";
            var queueCreationOptions
                = new RabbitQueueCreationOptions
                  {
                      QueueName = Guid.NewGuid().ToString(),
                      RetriesCount = 1,
                      RetryInitialTimeout = TimeSpan.FromMilliseconds(100),
                      ExceptionHandlingPolicy = ExceptionHandlingPolicy.None
                  };


            // When
            var messageHandler = new ThrowingExceptionMessageHandler();

            using (var queue = _queuesFactory.Create<string>(queueCreationOptions))
            {
                await queue.SendMessageAsync(expectedMessage);
                await queue.SubscribeAsync(messageHandler);

                await Task.Delay(_completionTimeout);
            }

            // Then
            _mockLogger.VerifyNoErrorsWasLogged();

            Assert.Equal(expectedMessage, messageHandler.Messages.Single());
            Assert.Equal(0UL, GetQueueMessagesCount(queueCreationOptions.QueueName));
        }

        [Fact]
        public async Task ShouldRequeueAndProcessBadMessage()
        {
            // Given
            const string expectedMessage = "Test message";
            var queueCreationOptions
                = new RabbitQueueCreationOptions
                  {
                      QueueName = Guid.NewGuid().ToString(),
                      RetriesCount = 0,
                      RetryInitialTimeout = TimeSpan.FromMilliseconds(100),
                      ExceptionHandlingPolicy = ExceptionHandlingPolicy.Requeue
                  };


            // When
            var messageHandler = new ThrowingExceptionMessageHandler();

            using (var queue = _queuesFactory.Create<string>(queueCreationOptions))
            {
                await queue.SendMessageAsync(expectedMessage);
                await queue.SubscribeAsync(messageHandler);

                await Task.Delay(_completionTimeout);
            }

            // Then
            _mockLogger.VerifyErrorWasLogged<InvalidOperationException>();

            Assert.Equal(expectedMessage, messageHandler.Messages.Single());
            Assert.Equal(0UL, GetQueueMessagesCount(queueCreationOptions.QueueName));
        }

        [Fact]
        public async Task ShouldRequeueBadMessageWithDelayAndProcessIt()
        {
            // Given
            const string expectedMessage = "Test message";

            var queueName = Guid.NewGuid().ToString();
            var messageRequeuingDelay = TimeSpan.FromSeconds(1);
            var queueCreationOptions
                = new RabbitQueueCreationOptions
                  {
                      QueueName = queueName,
                      RetriesCount = 0,
                      RetryInitialTimeout = TimeSpan.FromMilliseconds(100),
                      ExceptionHandler =
                          new RequeuingWithDelayExceptionHandler(
                              _queuesFactory,
                              _loggerFactory.CreateLogger<RequeuingWithDelayExceptionHandler>(),
                              queueName,
                              messageRequeuingDelay
                          )
                  };


            // When
            var messageHandler = new ThrowingExceptionMessageHandler();

            using (var queue = _queuesFactory.Create<string>(queueCreationOptions))
            {
                await queue.SendMessageAsync(expectedMessage);
                await queue.SubscribeAsync(messageHandler);

                await Task.Delay((int)(messageRequeuingDelay.TotalMilliseconds / 2));
                Assert.Equal(0UL, GetQueueMessagesCount(queueCreationOptions.QueueName));

                await Task.Delay(_completionTimeout);
            }

            // Then
            _mockLogger.VerifyErrorWasLogged<InvalidOperationException>();

            Assert.Equal(expectedMessage, messageHandler.Messages.Single());
            Assert.Equal(0UL, GetQueueMessagesCount(queueCreationOptions.QueueName));
        }

        [Fact]
        public async Task ShouldPushBadMessageIntoErrorsQueue()
        {
            // Given
            const string expectedMessage = "Test message";
            var queueCreationOptions
                = new RabbitQueueCreationOptions
                  {
                      QueueName = Guid.NewGuid().ToString(),
                      RetriesCount = 0,
                      RetryInitialTimeout = TimeSpan.FromMilliseconds(100),
                      ExceptionHandlingPolicy = ExceptionHandlingPolicy.SendToErrorsQueue
                  };

            var errorQueueCreationOptions
                = new RabbitQueueCreationOptions
                  {
                      QueueName = $"{queueCreationOptions.QueueName}.Errors",
                      RetriesCount = 0,
                      RetryInitialTimeout = TimeSpan.FromMilliseconds(100),
                      ExceptionHandlingPolicy = ExceptionHandlingPolicy.None
                  };

            // When
            var messageHandler = new ThrowingExceptionMessageHandler();
            using (var queue = _queuesFactory.Create<string>(queueCreationOptions))
            {
                await queue.SubscribeAsync(messageHandler);
                await queue.SendMessageAsync(expectedMessage);

                await Task.Delay(_completionTimeout);
            }

            var errorMessagesHandler = new CatchingMessageHandler<ExceptionDescription>();
            using (var errorsQueue = _queuesFactory.Create<ExceptionDescription>(errorQueueCreationOptions))
            {
                await errorsQueue.SubscribeAsync(errorMessagesHandler);
                await Task.Delay(_completionTimeout);
            }

            // Then
            _mockLogger.VerifyErrorWasLogged<InvalidOperationException>();

            Assert.Empty(messageHandler.Messages);
            Assert.Equal(0UL, GetQueueMessagesCount(queueCreationOptions.QueueName));

            Assert.Contains(expectedMessage, errorMessagesHandler.Messages.Single().MessageContent);
            Assert.Equal(0UL, GetQueueMessagesCount(errorQueueCreationOptions.QueueName));
        }

        [Fact]
        public void ShouldDisposeObjectsIfExceptionInConstructorOccures()
        {
            // Given
            var mockConnection = new Mock<IConnection>();
            var mockModel = new Mock<IModel>();
            mockModel
                .Setup(x => x.QueueDeclare(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>()))
                .Throws(new Exception());
            mockConnection.Setup(x => x.CreateModel()).Returns(mockModel.Object);
            var mockErrorsQueue = new Mock<ITypedQueue<ExceptionDescription>>();
            var queuesFactory = new QueuesFactoryForTests(
                new ExceptionHandlersFactory<RabbitMessageDescription>(
                    new[]
                    {
                        new ErrorsQueuingExceptionHandler<RabbitMessageDescription>(
                            _loggerFactory.CreateLogger<ErrorsQueuingExceptionHandler<RabbitMessageDescription>>()
                        )
                    }
                ),
                _loggerFactory,
                Options.Create(
                    new TypedRabbitQueuesFactoryOptions
                    {
                        Credentials = new RabbitCredentianls {UserName = "guest", Password = "guest"}
                    }
                ),
                mockConnection.Object,
                mockErrorsQueue.Object
            );

            // When
            Assert.Throws<Exception>(
                () => queuesFactory.Create<string>(
                    new RabbitQueueCreationOptions
                    {
                        QueueName = "TestQueue",
                        RetryInitialTimeout = TimeSpan.FromMilliseconds(100),
                        ExceptionHandlingPolicy = ExceptionHandlingPolicy.SendToErrorsQueue
                    }
                )
            );

            // Then
            mockConnection.Verify(x => x.Dispose(), Times.Once);
            mockErrorsQueue.Verify(x => x.Dispose(), Times.Once);
            mockModel.Verify(x => x.Dispose(), Times.Once);
        }
    }
}