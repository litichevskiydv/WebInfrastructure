namespace Skeleton.Web.Authentication.JwtBearer.Configuration
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public class JwtBearerTokensIssuerBuilder : IJwtBearerTokensIssuerBuilder
    {
        public JwtBearerTokensIssuerBuilder(IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}