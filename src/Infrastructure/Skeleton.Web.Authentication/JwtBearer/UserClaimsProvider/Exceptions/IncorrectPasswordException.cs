namespace Skeleton.Web.Authentication.JwtBearer.UserClaimsProvider.Exceptions
{
    using System;

    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException(string login) : base($"Password is incorrect for login {login}")
        {
        }
    }
}