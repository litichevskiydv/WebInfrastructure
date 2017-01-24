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

            _next = next;
            _options = options.Value;
        }
    }
}