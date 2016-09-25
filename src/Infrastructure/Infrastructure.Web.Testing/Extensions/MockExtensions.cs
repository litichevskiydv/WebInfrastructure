namespace Infrastructure.Web.Testing.Extensions
{
    using System;
    using Microsoft.Extensions.Logging;
    using Moq;

    public static class MockExtensions
    {
        public static void VerifyNoErrors(this Mock<ILogger> mockLogger)
        {
            mockLogger.Verify(
                x => x.Log(It.Is<LogLevel>(l => l == LogLevel.Critical || 
                l == LogLevel.Error || l == LogLevel.Warning),
                    It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()),
                Times.Never);
        }
    }
}