namespace Skeleton.Queues.RabbitMq.Tests
{
    using System;
    using Abstractions.QueuesFactory.Configuration;
    using Abstractions.QueuesFactory.ExceptionsHandling.Handlers;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using Moq;
    using QueuesFactory;
    using QueuesFactory.Configuration;
    using Xunit;

    public class QueueCreationOptionsExtensionsTests
    {
        #region TestCases

        public class ExceptionHandlerParametersValidationTestCase
        {
            public RabbitQueueCreationOptions QueueCreationOptions { get; set; }

            public ExceptionHandlerBase<RabbitMessageDescription> ExceptionHandler { get; set; }
        }

        #endregion

        [UsedImplicitly]
        public static TheoryData<ExceptionHandlerParametersValidationTestCase> ExceptionHandlerParametersValidationTestCases;

        static QueueCreationOptionsExtensionsTests()
        {
            ExceptionHandlerParametersValidationTestCases =
                new TheoryData<ExceptionHandlerParametersValidationTestCase>
                {
                    new ExceptionHandlerParametersValidationTestCase
                    {
                        ExceptionHandler = new EmptyExceptionHandler<RabbitMessageDescription>(
                            new Mock<ILogger<EmptyExceptionHandler<RabbitMessageDescription>>>().Object
                        )
                    },
                    new ExceptionHandlerParametersValidationTestCase
                    {
                        QueueCreationOptions = new RabbitQueueCreationOptions(),
                    }
                };
        }

        [Theory]
        [MemberData(nameof(ExceptionHandlerParametersValidationTestCases))]
        public void WithExceptionHandlerShouldValidateParameters(ExceptionHandlerParametersValidationTestCase testCase)
        {
            Assert.Throws<ArgumentNullException>(() => testCase.QueueCreationOptions.WithExceptionHandler(testCase.ExceptionHandler));
        }

        [Fact]
        public void ShouldConfigureExceptionHandler()
        {
            // Given
            var queueCreationOptions = new RabbitQueueCreationOptions();
            var expectedExceptionHandler = new EmptyExceptionHandler<RabbitMessageDescription>(
                new Mock<ILogger<EmptyExceptionHandler<RabbitMessageDescription>>>().Object
            );

            // When
            queueCreationOptions.WithExceptionHandler(expectedExceptionHandler);

            // Then
            Assert.Equal(expectedExceptionHandler, queueCreationOptions.ExceptionHandler);
        }
    }
}