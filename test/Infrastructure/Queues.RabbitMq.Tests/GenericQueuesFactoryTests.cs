namespace Skeleton.Queues.RabbitMq.Tests
{
    using System;
    using System.Collections.Generic;
    using Abstractions.QueuesFactory;
    using Abstractions.QueuesFactory.ExceptionsHandling;
    using Abstractions.QueuesFactory.ExceptionsHandling.Handlers;
    using JetBrains.Annotations;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using QueuesFactory;
    using QueuesFactory.Configuration;
    using Xunit;

    public class GenericQueuesFactoryTests
    {
        [UsedImplicitly]
        public static IEnumerable<object[]> GenericQueuesFactoryConstructorParametersValidationTestsData;

        static GenericQueuesFactoryTests()
        {
            GenericQueuesFactoryConstructorParametersValidationTestsData =
                new[]
                {
                    new object[] {null}
                };
        }

        [Theory]
        [MemberData(nameof(GenericQueuesFactoryConstructorParametersValidationTestsData))]
        public void GenericQueuesFactoryConstructorShouldValidateParameters(IServiceProvider serviceProvider)
        {
            Assert.Throws<ArgumentNullException>(() => new GenericQueuesFactory(serviceProvider));
        }

        [Fact]
        public void GenericQueuesFactoryShouldPassInnerExceptionsCorrectly()
        {
            // Given
            var genericQueuesFactory
                = new GenericQueuesFactory(
                    new ServiceCollection()
                        .AddSingleton<ITypedQueuesFactory<RabbitQueueCreationOptions>>(
                            new TypedRabbitQueuesFactory(
                                new ExceptionHandlersFactory<RabbitMessageDescription>(
                                    new ExceptionHandlerBase<RabbitMessageDescription>[]
                                    {
                                        new EmptyExceptionHandler<RabbitMessageDescription>(
                                            new Mock<ILogger<EmptyExceptionHandler<RabbitMessageDescription>>>().Object
                                        )

                                    }
                                ),
                                new Mock<ILoggerFactory>().Object,
                                Options.Create(
                                    new TypedRabbitQueuesFactoryOptions
                                    {
                                        Credentials = new RabbitCredentianls {UserName = "guest", Password = "guest"},
                                        NetworkRecoveryInterval = TimeSpan.FromSeconds(30)
                                    }
                                )
                            )
                        )
                        .BuildServiceProvider()
                );

            // When, Then
            Assert.Throws<ArgumentException>(
                () => genericQueuesFactory.Create<string>(
                    new RabbitQueueCreationOptions {QueueName = "Test", RetryInitialTimeout = TimeSpan.FromSeconds(30)}
                )
            );
        }
    }
}