namespace Skeleton.Web.Authentication.JwtBearer.Configuration
{
    using System;
    using System.Collections.Generic;
    using Common.Extensions;
    using Microsoft.IdentityModel.Tokens;

    public static class TokenValidationParametersExtensions
    {
        public static TokenValidationParameters WithIssuerKeyValidation(this TokenValidationParameters parameters, 
            SecurityKey securityKey, IssuerSigningKeyValidator keyValidator = null)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (securityKey == null)
                throw new ArgumentNullException(nameof(securityKey));


            parameters.ValidateIssuerSigningKey = true;
            parameters.IssuerSigningKey = securityKey;
            parameters.IssuerSigningKeyValidator = keyValidator;

            return parameters;
        }

        public static TokenValidationParameters WithIssuerKeyValidation(this TokenValidationParameters parameters,
            IssuerSigningKeyResolver securityKeyResolver, IssuerSigningKeyValidator keyValidator = null)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (securityKeyResolver == null)
                throw new ArgumentNullException(nameof(securityKeyResolver));


            parameters.ValidateIssuerSigningKey = true;
            parameters.IssuerSigningKeyResolver = securityKeyResolver;
            parameters.IssuerSigningKeyValidator = keyValidator;

            return parameters;
        }

        public static TokenValidationParameters WithIssuerKeyValidation(this TokenValidationParameters parameters,
            IEnumerable<SecurityKey> securityKeys, IssuerSigningKeyValidator keyValidator = null)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            var securityKeysArray = securityKeys.AsArray();
            if (securityKeysArray.Length == 0)
                throw new ArgumentNullException(nameof(securityKeys));

            parameters.ValidateIssuerSigningKey = true;
            parameters.IssuerSigningKeys = securityKeysArray;
            parameters.IssuerSigningKeyValidator = keyValidator;

            return parameters;
        }

        public static TokenValidationParameters WithoutIssuerKeyValidation(this TokenValidationParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            parameters.ValidateIssuerSigningKey = false;
            return parameters;
        }

        public static TokenValidationParameters WithLifetimeValidation(this TokenValidationParameters parameters,
            LifetimeValidator lifetimeValidator = null)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            parameters.ValidateLifetime = true;
            parameters.LifetimeValidator = lifetimeValidator;
            return parameters;
        }

        public static TokenValidationParameters WithoutLifetimeValidation(this TokenValidationParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            parameters.ValidateLifetime = false;
            return parameters;
        }

        public static TokenValidationParameters WithIssuerValidation(this TokenValidationParameters parameters,
            string validIssuer)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(validIssuer))
                throw new ArgumentNullException(nameof(validIssuer));

            parameters.ValidateIssuer = true;
            parameters.ValidIssuer = validIssuer;
            return parameters;
        }

        public static TokenValidationParameters WithIssuerValidation(this TokenValidationParameters parameters,
            IEnumerable<string> validIssuers)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            var validIssuersArray = validIssuers.AsArray();
            if (validIssuersArray.Length == 0)
                throw new ArgumentNullException(nameof(validIssuers));

            parameters.ValidateIssuer = true;
            parameters.ValidIssuers = validIssuersArray;
            return parameters;
        }

        public static TokenValidationParameters WithoutIssuerValidation(this TokenValidationParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            parameters.ValidateIssuer = false;
            return parameters;
        }

        public static TokenValidationParameters WithAudienceValidation(this TokenValidationParameters parameters,
            string validAudience)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(validAudience))
                throw new ArgumentNullException(nameof(validAudience));

            parameters.ValidateAudience = true;
            parameters.ValidAudience = validAudience;
            return parameters;
        }

        public static TokenValidationParameters WithAudienceValidation(this TokenValidationParameters parameters,
            IEnumerable<string> validAudiences)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            var validAudiencesArray = validAudiences.AsArray();
            if (validAudiencesArray.Length == 0)
                throw new ArgumentNullException(nameof(validAudiences));

            parameters.ValidateAudience = true;
            parameters.ValidAudiences = validAudiencesArray;
            return parameters;
        }

        public static TokenValidationParameters WithoutAudienceValidation(this TokenValidationParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            parameters.ValidateAudience = false;
            return parameters;
        }
    }
}