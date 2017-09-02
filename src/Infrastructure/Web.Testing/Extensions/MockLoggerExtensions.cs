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

        public static Mock<ILogger> VerifyNoErrorsWasLogged(this Mock<ILogger> mockLogger)
        {
            mockLogger.Verify(
                x => x.Log(It.Is<LogLevel>(l => l == LogLevel.Critical || l == LogLevel.Error),
                    It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()),
                Times.Never);
            return mockLogger;
        }

        public static Mock<ILogger> VerifyErrorWasLogged<TException>(this Mock<ILogger> mockLogger) where TException : Exception
        {
            mockLogger.Verify(
                x => x.Log(It.Is<LogLevel>(l => l == LogLevel.Critical || l == LogLevel.Error),
                    It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<TException>(), It.IsAny<Func<object, Exception, string>>()),
                Times.Once);
            return mockLogger;
        }

        public static Mock<ILogger> VerifyNoWarningsWasLogged(this Mock<ILogger> mockLogger)
        {
            mockLogger.Verify(
                x => x.Log(It.Is<LogLevel>(l => l == LogLevel.Warning),
                    It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()),
                Times.Never);
            return mockLogger;
        }

        public static Mock<ILogger> VerifyWarningWasLogged(this Mock<ILogger> mockLogger)
        {
            mockLogger.Verify(
                x => x.Log(It.Is<LogLevel>(l => l == LogLevel.Warning),
                    It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()),
                Times.Once);
            return mockLogger;
        }
    }
}