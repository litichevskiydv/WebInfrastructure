namespace Skeleton.Web.Authentication.JwtBearer.Configuration
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IJwtBearerTokensIssuerBuilder
    {
        IServiceCollection Services { get; }
    }
}