namespace Skeleton.Web.Authentication.JwtBearer.Configuration
{
    using System;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;

    public static class JwtBearerOptionsExtensions
    {
        public static JwtBearerOptions WithTokenValidationParameters(this JwtBearerOptions options,
            Func<TokenValidationParameters, TokenValidationParameters> parametersBuilder)
        {
            if(options == null)
                throw new ArgumentNullException(nameof(options));
            if (parametersBuilder == null)
                throw new ArgumentNullException(nameof(parametersBuilder));

            options.TokenValidationParameters = parametersBuilder(options.TokenValidationParameters);
            return options;
        }

        public static JwtBearerOptions WithEventsProcessor(this JwtBearerOptions options, JwtBearerEvents eventsProcessor)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (eventsProcessor == null)
                throw new ArgumentNullException(nameof(eventsProcessor));

            options.Events = eventsProcessor;
            return options;
        }

        public static JwtBearerOptions WithErrorDetails(this JwtBearerOptions options, bool includeErrorDetails = true)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.IncludeErrorDetails = includeErrorDetails;
            return options;
        }

        public static JwtBearerOptions WithoutErrorDetails(this JwtBearerOptions options)
        {
            return options.WithErrorDetails(false);
        }
    }
}