namespace Skeleton.Web.Authorisation.JwtBearer.Configuration
{
    using System;
    using Microsoft.IdentityModel.Tokens;

    public static class TokensIssuingOptionsExtensions
    {
        public static TokensIssuingOptions WithGetEndpotint(this TokensIssuingOptions options, string endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentNullException(nameof(endpoint));

            options.GetEndpotint = endpoint;
            return options;
        }

        public static TokensIssuingOptions WithSigningKey(this TokensIssuingOptions options, string signingAlgorithmName, SecurityKey signingKey)
        {
            options.SigningKey = signingKey;
            options.SigningAlgorithmName = signingAlgorithmName;
            return options;
        }

        public static TokensIssuingOptions WithLifeTime(this TokensIssuingOptions options, TimeSpan? lifeTime)
        {
            options.LifeTime = lifeTime;
            return options;
        }
    }
}