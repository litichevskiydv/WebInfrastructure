namespace Skeleton.Web.Authentication.JwtBearer.Configuration.Issuing
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;

    public static class JwtBearerTokensIssuerBuilderExtensions
    {
        public static IJwtBearerTokensIssuerBuilder WithTokenIssueEventHandler<TEventHandler>(
            this IJwtBearerTokensIssuerBuilder issuerBuilder
        )
            where TEventHandler : ITokenIssueEventHandler
        {
            if (issuerBuilder == null)
                throw new ArgumentNullException(nameof(issuerBuilder));

            issuerBuilder.Services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<TokensIssuingOptions>, TokenIssueEventHandlerSetup<TEventHandler>>()
            );
            return issuerBuilder;
        }
    }
}