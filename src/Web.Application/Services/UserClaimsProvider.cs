namespace Web.Application.Services
{
    using System.Security.Claims;
    using JetBrains.Annotations;
    using Skeleton.Web.Authorisation.JwtBearer.UserClaimsProvider;
    using Skeleton.Web.Authorisation.JwtBearer.UserClaimsProvider.Exceptions;

    [UsedImplicitly]
    public class UserClaimsProvider : IUserClaimsProvider
    {
        public Claim[] GetClaims(string login, string password)
        {
            if (login == "lhp@lhp.com")
            {
                if (password != "1234")
                    throw new IncorrectPasswordException(login);
                return new[] {new Claim(ClaimTypes.Email, login)};
            }

            throw new LoginNotFoundException(login);
        }
    }
}