namespace Skeleton.Web.Authorisation.JwtBearer.UserClaimsProvider
{
    using System.Security.Claims;

    public interface IUserClaimsProvider
    {
        Claim[] GetClaims(string login, string password);
    }
}