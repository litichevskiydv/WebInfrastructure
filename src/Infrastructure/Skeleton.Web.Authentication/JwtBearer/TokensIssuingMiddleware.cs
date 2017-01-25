namespace Skeleton.Web.Authorisation.JwtBearer
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    public class TokensIssuingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokensIssuingOptions _options;

        public TokensIssuingMiddleware(RequestDelegate next, IOptions<TokensIssuingOptions> options)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(options.Value.GetEndpotint))
                throw new ArgumentNullException(nameof(options.Value.GetEndpotint));
            if (string.IsNullOrWhiteSpace(options.Value.SigningAlgorithmName))
                throw new ArgumentNullException(nameof(options.Value.SigningAlgorithmName));
            if (options.Value.SigningKey == null)
                throw new ArgumentNullException(nameof(options.Value.SigningKey));

            _next = next;
            _options = options.Value;
        }
    }
}