namespace Skeleton.Web.Authentication.JwtBearer.Configuration
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IJwtBearerTokensIssuerBuilder AddJwtBearerTokensIssuer(this IServiceCollection services, Action<TokensIssuingOptions> configure)
        {
            if(services == null)
                throw new ArgumentNullException(nameof(services));
            if(configure == null)
                throw new ArgumentNullException(nameof(configure));

            services
                .AddOptions()
                .Configure(configure);

            return new JwtBearerTokensIssuerBuilder(services);
        }
    }
}