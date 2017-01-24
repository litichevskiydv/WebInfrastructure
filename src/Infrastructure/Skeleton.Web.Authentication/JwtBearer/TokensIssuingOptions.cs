namespace Skeleton.Web.Authorisation.JwtBearer
{
    using System;
    using Microsoft.IdentityModel.Tokens;

    public class TokensIssuingOptions
    {
        public string GetEndpotint { get; set; }

        public SecurityKey SigningKey { get; set; }

        public string SigningAlgorithmName { get; set; }

        public TimeSpan? LifeTime { get; set; }
    }
}