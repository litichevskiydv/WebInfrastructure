namespace Skeleton.Web.Authorisation.JwtBearer.Configuration
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseJwtBearerAuthorisationTokens(this IServiceCollection services,
            Func<IJwtBearerAuthorisationConfigurator, IJwtBearerAuthorisationConfigurator> configurationBuilder)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configurationBuilder == null)
                throw new ArgumentNullException(nameof(configurationBuilder));

            var configurator = new JwtBearerAuthorisationConfigurator();
            configurationBuilder(configurator);
            return services
                .AddOptions()
                .Configure<JwtBearerOptions>(x => configurator.JwtBearerOptionsBuilder(x))
                .Configure<TokensIssuingOptions>(x => configurator.TokensIssuingOptionsBuilder(x));
        }
    }
}