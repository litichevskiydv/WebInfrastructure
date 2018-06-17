namespace Skeleton.Web.Testing.Extensions
{
    using System;
    using Microsoft.Extensions.Logging;
    using Moq;

    public static class MockLoggerExtensions
    {
        public static Mock<ILogger> CreateMockLogger()
        {
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
            return mockLogger;
        }

        public static Mock<TLogger> VerifyNoErrorsWasLogged<TLogger>(this Mock<TLogger> mockLogger) where TLogger : class, ILogger
        {
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                ),
                Times.Never
            );
            return mockLogger;
        }

        public static Mock<ILogger> VerifyErrorWasLogged<TException>(this Mock<ILogger> mockLogger)
            where TException : Exception
        {
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<TException>(),
                    It.IsAny<Func<object, Exception, string>>()
                ),
                Times.Once
            );
            return mockLogger;
        }

        public static Mock<TLogger> VerifyErrorWasLogged<TLogger>(this Mock<TLogger> mockLogger, Type exceptionType)
            where TLogger : class, ILogger
        {
            if (typeof(Exception).IsAssignableFrom(exceptionType) == false)
                throw new InvalidOperationException("Specified type was not inherited from Exception");

            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.Is<Exception>(e => e.GetType() == exceptionType),
                    It.IsAny<Func<object, Exception, string>>()
                ),
                Times.Once
            );
            return mockLogger;
        }

        public static Mock<TLogger> VerifyNoWarningsWasLogged<TLogger>(this Mock<TLogger> mockLogger) where TLogger : class, ILogger
        {
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                ),
                Times.Never
            );
            return mockLogger;
        }

        public static Mock<TLogger> VerifyWarningWasLogged<TLogger>(this Mock<TLogger> mockLogger) where TLogger : class, ILogger
        {
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                ),
                Times.Once
            );
            return mockLogger;
        }
    }
}