namespace Skeleton.Integrations.WebApiClient.Exceptions
{
    using System;
    using JetBrains.Annotations;

    public class NotFoundException : Exception
    {
        [UsedImplicitly]
        public NotFoundException(string message) : base(message)
        {
        }

        [UsedImplicitly]
        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
