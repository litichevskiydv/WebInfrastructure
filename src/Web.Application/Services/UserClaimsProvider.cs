namespace Web.Application.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Skeleton.Web.Authentication.JwtBearer.UserClaimsProvider;
    using Skeleton.Web.Authentication.JwtBearer.UserClaimsProvider.Exceptions;

    [UsedImplicitly]
    public class UserClaimsProvider : IUserClaimsProvider
    {
        public Task<Claim[]> GetClaimsAsync(string login, string password)
        {
            if (login == "lhp@lhp.com")
            {
                if (password != "1234")
                    throw new IncorrectPasswordException(login);
                return Task.FromResult(new[] {new Claim(ClaimTypes.Email, login)});
            }

            throw new LoginNotFoundException(login);
        }
    }
}