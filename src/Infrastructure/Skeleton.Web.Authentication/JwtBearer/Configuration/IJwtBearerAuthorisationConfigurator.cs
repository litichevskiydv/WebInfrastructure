namespace Skeleton.Web.Authorisation.JwtBearer.Configuration
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.IdentityModel.Tokens;

    public interface IJwtBearerAuthorisationConfigurator
    {
        IJwtBearerAuthorisationConfigurator ConfigureJwtBearerOptions(Func<JwtBearerOptions, JwtBearerOptions> builder);

        IJwtBearerAuthorisationConfigurator ConfigureTokensIssuingOptions(Func<TokensIssuingOptions, TokensIssuingOptions> builder);

        IJwtBearerAuthorisationConfigurator ConfigureSigningKey(string signingAlgorithmName, SecurityKey signingKey);
    }
}