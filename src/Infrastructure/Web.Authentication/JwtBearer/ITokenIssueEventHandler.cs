namespace Skeleton.Web.Authentication.JwtBearer
{
    using System;
    using System.Security.Claims;

    public interface ITokenIssueEventHandler
    {
        void IssueSuccessEventHandle(string token, Claim[] user);
        void LoginNotFoundEventHandle(string login);
        void IncorrectPasswordEventHandle(string login, string password);
    }
}
