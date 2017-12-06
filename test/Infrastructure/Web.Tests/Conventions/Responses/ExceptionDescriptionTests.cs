namespace Skeleton.Web.Tests.Conventions.Responses
{
    using System;
    using Web.Conventions.Responses;
    using Xunit;

    public class ExceptionDescriptionTests
    {
        [Fact]
        public void ShouldCollectInformation()
        {
            // Given
            const string message = "<Message>";
            const string innerMessage = "<Inner_Message>";

            void Action()
            {
                void InnerAction() => throw new ArgumentException(innerMessage);
                try
                {
                    InnerAction();
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException(message, e);
                }
            }

            //When
            Exception exception = null;
            try
            {
                Action();
            }
            catch (Exception e)
            {
                exception = e;
            }
            var exceptionStringDescription = new ExceptionDescription(exception).ToString();

            // Then
            Assert.Contains(message, exceptionStringDescription, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains(exception.GetType().ToString(), exceptionStringDescription, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains(exception.StackTrace, exceptionStringDescription, StringComparison.InvariantCultureIgnoreCase);

            Assert.Contains(innerMessage, exceptionStringDescription, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains(exception.InnerException.GetType().ToString(), exceptionStringDescription, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains(exception.InnerException.StackTrace, exceptionStringDescription, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}