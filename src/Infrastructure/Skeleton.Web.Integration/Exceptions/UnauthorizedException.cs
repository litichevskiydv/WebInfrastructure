namespace Skeleton.Web.Integration.Exceptions
{
    using System;
    using JetBrains.Annotations;

    public class UnauthorizedException : Exception
    {
        [UsedImplicitly]
        public UnauthorizedException(string message) : base(message)
        {
        }

        [UsedImplicitly]
        public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}