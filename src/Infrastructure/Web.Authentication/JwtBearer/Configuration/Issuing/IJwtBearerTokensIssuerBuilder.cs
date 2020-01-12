namespace Skeleton.Web.Authentication.JwtBearer.Configuration.Issuing
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IJwtBearerTokensIssuerBuilder
    {
        IServiceCollection Services { get; }
    }
}