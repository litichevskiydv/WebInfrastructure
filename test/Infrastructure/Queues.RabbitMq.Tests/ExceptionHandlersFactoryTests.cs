namespace Skeleton.Queues.RabbitMq.Tests
{
    using System;
    using Abstractions.QueuesFactory.ExceptionsHandling;
    using Abstractions.QueuesFactory.ExceptionsHandling.Handlers;
    using Microsoft.Extensions.Logging;
    using Moq;
    using QueuesFactory;
    using Xunit;

    public class ExceptionHandlersFactoryTests
    {
        [Fact]
        public void ShouldNotAllowNullInsteadHandlersCollection()
        {
            Assert.Throws<ArgumentNullException>(() => new ExceptionHandlersFactory<RabbitMessageDescription>(null));
        }

        [Fact]
        public void ShouldNotAllowTwoHandlersForSamePolicy()
        {
            Assert.Throws<InvalidOperationException>(
                () => new ExceptionHandlersFactory<RabbitMessageDescription>(
                    new ExceptionHandlerBase<RabbitMessageDescription>[]
                    {
                        new EmptyExceptionHandler<RabbitMessageDescription>(
                            new Mock<ILogger<EmptyExceptionHandler<RabbitMessageDescription>>>().Object
                        ),
                        new EmptyExceptionHandler<RabbitMessageDescription>(
                            new Mock<ILogger<EmptyExceptionHandler<RabbitMessageDescription>>>().Object
                        )
                    }
                )
            );
        }

        [Fact]
        public void ShouldReturnAppropriateHandlerForPolicy()
        {
            // Given
            var handlersFactory =
                new ExceptionHandlersFactory<RabbitMessageDescription>(
                    new ExceptionHandlerBase<RabbitMessageDescription>[]
                    {
                        new EmptyExceptionHandler<RabbitMessageDescription>(
                            new Mock<ILogger<EmptyExceptionHandler<RabbitMessageDescription>>>().Object
                        )
                    }
                );

            // When
            var handler = handlersFactory.GetHandler(ExceptionHandlingPolicy.None);

            // Then
            Assert.Equal(ExceptionHandlingPolicy.None, handler.ExceptionHandlingPolicy);
        }

        [Fact]
        public void ShouldThrowExceptionIfAppropriateHandlerForPolicyNotExisted()
        {
            // Given
            var handlersFactory =
                new ExceptionHandlersFactory<RabbitMessageDescription>(
                    new ExceptionHandlerBase<RabbitMessageDescription>[0]
                );

            // When, Then
            Assert.Throws<NotSupportedException>(() => handlersFactory.GetHandler(ExceptionHandlingPolicy.None));
        }
    }
}