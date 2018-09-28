namespace Skeleton.Web.Authentication.JwtBearer.Configuration
{
    using System;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtBearerAuthorizationTokens(this IServiceCollection services,
            Func<IJwtBearerAuthenticationConfigurator, IJwtBearerAuthenticationConfigurator> configurationBuilder)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configurationBuilder == null)
                throw new ArgumentNullException(nameof(configurationBuilder));

            var configurator = new JwtBearerAuthenticationConfigurator();
            configurationBuilder(configurator);
            services
                .AddOptions()
                .Configure<TokensIssuingOptions>(x => configurator.TokensIssuingOptionsBuilder(x))
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x => configurator.JwtBearerOptionsBuilder(x));

            return services;
        }
    }
}