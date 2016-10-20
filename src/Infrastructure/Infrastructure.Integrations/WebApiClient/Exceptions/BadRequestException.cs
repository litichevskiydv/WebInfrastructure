namespace Infrastructure.Integrations.WebApiClient.Exceptions
{
    using System;
    using JetBrains.Annotations;

    public class BadRequestException : Exception
    {
        [UsedImplicitly]
        public BadRequestException(string message) : base(message)
        {
        }

        [UsedImplicitly]
        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}