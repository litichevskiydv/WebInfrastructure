namespace Skeleton.Web.Authentication.JwtBearer.Configuration
{
    using System;
    using Microsoft.IdentityModel.Tokens;

    public static class TokensIssuingOptionsExtensions
    {
        public static TokensIssuingOptions WithGetEndpoint(this TokensIssuingOptions options, string endpoint)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentNullException(nameof(endpoint));

            options.GetEndpoint = endpoint;
            return options;
        }

        public static TokensIssuingOptions WithSigningKey(this TokensIssuingOptions options, string signingAlgorithmName, SecurityKey signingKey)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if(string.IsNullOrWhiteSpace(signingAlgorithmName))
                throw new ArgumentNullException(nameof(signingAlgorithmName));
            if (signingKey == null)
                throw new ArgumentNullException(nameof(signingKey));

            options.SigningAlgorithmName = signingAlgorithmName;
            options.SigningKey = signingKey;
            return options;
        }

        public static TokensIssuingOptions WithLifetime(this TokensIssuingOptions options, TimeSpan lifetime)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.Lifetime = lifetime;
            return options;
        }

        public static TokensIssuingOptions WithTokenIssueEventHandler(this TokensIssuingOptions options, ITokenIssueEventHandler eventHandler)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (eventHandler == null)
                throw new ArgumentNullException(nameof(eventHandler));

            options.TokenIssueEventHandler = eventHandler;
            return options;
        }
    }
}