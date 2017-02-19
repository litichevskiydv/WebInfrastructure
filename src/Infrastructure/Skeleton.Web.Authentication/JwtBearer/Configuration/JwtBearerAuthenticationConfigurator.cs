namespace Skeleton.Web.Authentication.JwtBearer.Configuration
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.IdentityModel.Tokens;

    internal class JwtBearerAuthenticationConfigurator : IJwtBearerAuthenticationConfigurator
    {
        private Func<JwtBearerOptions, JwtBearerOptions> _jwtBearerOptionsBuilder;
        private Func<TokensIssuingOptions, TokensIssuingOptions> _tokensIssuingOptionsBuilder;

        private SecurityKey _signingKey;
        private string _signingAlgorithmName;

        public IJwtBearerAuthenticationConfigurator ConfigureJwtBearerOptions(Func<JwtBearerOptions, JwtBearerOptions> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            _jwtBearerOptionsBuilder = builder;
            return this;
        }

        public IJwtBearerAuthenticationConfigurator ConfigureTokensIssuingOptions(Func<TokensIssuingOptions, TokensIssuingOptions> builder)
        {
            if(builder == null)
                throw new ArgumentNullException(nameof(builder));

            _tokensIssuingOptionsBuilder = builder;
            return this;
        }

        public IJwtBearerAuthenticationConfigurator ConfigureSigningKey(string signingAlgorithmName, SecurityKey signingKey)
        {
            if(string.IsNullOrWhiteSpace(signingAlgorithmName))
                throw new ArgumentNullException(nameof(signingAlgorithmName));
            if(signingKey == null)
                throw new ArgumentNullException(nameof(signingKey));

            _signingAlgorithmName = signingAlgorithmName;
            _signingKey = signingKey;
            return this;
        }

        public Func<JwtBearerOptions, JwtBearerOptions> JwtBearerOptionsBuilder
        {
            get
            {
                if(_jwtBearerOptionsBuilder == null)
                    throw new InvalidOperationException($"{nameof(JwtBearerOptionsBuilder)} wasn't configured");

                return _signingKey == null
                    ? _jwtBearerOptionsBuilder
                    : x => _jwtBearerOptionsBuilder(x)
                          .WithTokenValidationParameters(p => p.WithIssuerKeyValidation(_signingKey));
            }
        }

        public JwtBearerOptions JwtBearerOptions => JwtBearerOptionsBuilder(new JwtBearerOptions());

        public Func<TokensIssuingOptions, TokensIssuingOptions> TokensIssuingOptionsBuilder
        {
            get
            {
                if(_tokensIssuingOptionsBuilder == null)
                    throw new InvalidOperationException($"{nameof(TokensIssuingOptionsBuilder)} wasn't configured");

                return _signingKey == null
                    ? _tokensIssuingOptionsBuilder
                    : x => _tokensIssuingOptionsBuilder(x)
                          .WithSigningKey(_signingAlgorithmName, _signingKey);
            }
        }

        public TokensIssuingOptions TokensIssuingOptions => TokensIssuingOptionsBuilder(new TokensIssuingOptions());
    }
}