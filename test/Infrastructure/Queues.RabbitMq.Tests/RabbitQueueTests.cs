﻿namespace Skeleton.Queues.RabbitMq.Tests
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
    using RabbitMQ.Client;
    using Web.Configuration;
    using Web.Testing.Extensions;
    using Xunit;

    public class RabbitQueueTests
    {
        private readonly TimeSpan _completionTimeout;
        private readonly Mock<ILogger> _mockLogger;
        private readonly IConnectionFactory _connectionsFactory;

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
            var queuesFactoryOptions = configuration.GetSection("QueuesFactoryOptions").Get<TypedRabbitQueuesFactoryOptions>();

            _connectionsFactory
                = new ConnectionFactory
                  {
                      UserName = queuesFactoryOptions.Credentianls.UserName,
                      Password = queuesFactoryOptions.Credentianls.Password,
                      AutomaticRecoveryEnabled = true,
                      NetworkRecoveryInterval = queuesFactoryOptions.NetworkRecoveryInterval,
                      TopologyRecoveryEnabled = true,
                      HostName = queuesFactoryOptions.Hosts.Single()
                };
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
    }
}