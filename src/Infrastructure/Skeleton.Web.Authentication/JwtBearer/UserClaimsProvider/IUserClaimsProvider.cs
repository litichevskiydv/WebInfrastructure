namespace Skeleton.Web.Authentication.JwtBearer.UserClaimsProvider
{
    using System.Security.Claims;

    public interface IUserClaimsProvider
    {
        Claim[] GetClaims(string login, string password);
    }
}