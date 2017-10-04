namespace Skeleton.Web.Integration.BaseApiClient.Exceptions
{
    using System;

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}