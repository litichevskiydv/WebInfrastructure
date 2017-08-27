namespace Skeleton.Web.Authentication.JwtBearer.Configuration
{
    using System;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;

    public interface IJwtBearerAuthenticationConfigurator
    {
        IJwtBearerAuthenticationConfigurator ConfigureJwtBearerOptions(Func<JwtBearerOptions, JwtBearerOptions> builder);

        IJwtBearerAuthenticationConfigurator ConfigureTokensIssuingOptions(Func<TokensIssuingOptions, TokensIssuingOptions> builder);

        IJwtBearerAuthenticationConfigurator ConfigureSigningKey(string signingAlgorithmName, SecurityKey signingKey);
    }
}