namespace Skeleton.Web.Testing.Extensions
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

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
            Assert.Null(
                mockLogger.Invocations.SingleOrDefault(
                    x => x.Arguments[0].Equals(LogLevel.Error)
                )
            );
            return mockLogger;
        }

        public static Mock<ILogger> VerifyErrorWasLogged<TException>(this Mock<ILogger> mockLogger)
            where TException : Exception
        {
            Assert.NotNull(
                mockLogger.Invocations.SingleOrDefault(
                    x => x.Arguments[0].Equals(LogLevel.Error)
                         && x.Arguments[3] != null
                         && x.Arguments[3].GetType() == typeof(TException)
                )
            );
            return mockLogger;
        }

        public static Mock<TLogger> VerifyErrorWasLogged<TLogger>(this Mock<TLogger> mockLogger, Type exceptionType)
            where TLogger : class, ILogger
        {
            if (typeof(Exception).IsAssignableFrom(exceptionType) == false)
                throw new InvalidOperationException("Specified type was not inherited from Exception");

            Assert.NotNull(
                mockLogger.Invocations.SingleOrDefault(
                    x => x.Arguments[0].Equals(LogLevel.Error)
                         && x.Arguments[3] != null
                         && x.Arguments[3].GetType() == exceptionType
                ));
            return mockLogger;
        }

        public static Mock<TLogger> VerifyNoWarningsWasLogged<TLogger>(this Mock<TLogger> mockLogger) where TLogger : class, ILogger
        {
            Assert.Null(
                mockLogger.Invocations.SingleOrDefault(
                    x => x.Arguments[0].Equals(LogLevel.Warning)
                )
            );
            return mockLogger;
        }

        public static Mock<TLogger> VerifyWarningWasLogged<TLogger>(this Mock<TLogger> mockLogger) where TLogger : class, ILogger
        {
            Assert.NotNull(
                mockLogger.Invocations.SingleOrDefault(
                    x => x.Arguments[0].Equals(LogLevel.Warning)
                )
            );
            return mockLogger;
        }
    }
}