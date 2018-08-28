namespace Skeleton.Queues.RabbitMq.Tests
{
    using System;
    using System.Collections.Generic;
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
        [UsedImplicitly]
        public static IEnumerable<object[]> WithExceptionHandlerParametersValidationTestsData;

        static QueueCreationOptionsExtensionsTests()
        {
            WithExceptionHandlerParametersValidationTestsData =
                new[]
                {
                    new object[]
                    {
                        null,
                        new EmptyExceptionHandler<RabbitMessageDescription>(
                            new Mock<ILogger<EmptyExceptionHandler<RabbitMessageDescription>>>().Object
                        )
                    },
                    new object[]
                    {
                        new RabbitQueueCreationOptions(),
                        null
                    }
                };
        }

        [Theory]
        [MemberData(nameof(WithExceptionHandlerParametersValidationTestsData))]
        public void WithExceptionHandlerShouldValidateParameters(
            RabbitQueueCreationOptions queueCreationOptions,
            ExceptionHandlerBase<RabbitMessageDescription> exceptionHandler)
        {
            Assert.Throws<ArgumentNullException>(() => queueCreationOptions.WithExceptionHandler(exceptionHandler));
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