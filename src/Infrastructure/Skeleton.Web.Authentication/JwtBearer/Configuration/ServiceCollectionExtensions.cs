namespace Skeleton.Web.Authentication.JwtBearer.Configuration
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtBearerAuthorisationTokens(this IServiceCollection services,
            Func<IJwtBearerAuthenticationConfigurator, IJwtBearerAuthenticationConfigurator> configurationBuilder)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configurationBuilder == null)
                throw new ArgumentNullException(nameof(configurationBuilder));

            var configurator = new JwtBearerAuthenticationConfigurator();
            configurationBuilder(configurator);
            return services
                .AddOptions()
                .Configure<JwtBearerOptions>(x => configurator.JwtBearerOptionsBuilder(x))
                .Configure<TokensIssuingOptions>(x => configurator.TokensIssuingOptionsBuilder(x));
        }
    }
}