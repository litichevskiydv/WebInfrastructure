namespace Skeleton.Web.Authentication.JwtBearer.UserClaimsProvider.Exceptions
{
    using System;

    public class LoginNotFoundException : Exception
    {
        public LoginNotFoundException(string login) : base($"Login {login} wasn't found")
        {
        }
    }
}