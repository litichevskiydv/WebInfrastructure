namespace Skeleton.Queues.RabbitMq.Tests
{
    using System;
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
        #region TestCases

        public class ConstructorParametersValidationTestCase
        {
            public IServiceProvider ServiceProvider { get; set; }
        }

        #endregion

        [UsedImplicitly]
        public static TheoryData<ConstructorParametersValidationTestCase> ConstructorParametersValidationTestCases;

        static GenericQueuesFactoryTests()
        {
            ConstructorParametersValidationTestCases =
                new TheoryData<ConstructorParametersValidationTestCase>
                {
                    new ConstructorParametersValidationTestCase()
                };
        }

        [Theory]
        [MemberData(nameof(ConstructorParametersValidationTestCases))]
        public void GenericQueuesFactoryConstructorShouldValidateParameters(ConstructorParametersValidationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => new GenericQueuesFactory(testCase.ServiceProvider));
        }

        [Fact]
        public void GenericQueuesFactoryShouldPassInnerExceptionsCorrectly()
        {
            // Given
            var genericQueuesFactory
                = new GenericQueuesFactory(
                    new ServiceCollection()
                        .AddSingleton<ITypedQueuesFactory<RabbitQueueCreationOptions>>(
                            new RabbitQueuesFactory(
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