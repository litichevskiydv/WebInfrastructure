namespace Skeleton.Web.Integration.BaseApiClient.Exceptions
{
    using System;

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
