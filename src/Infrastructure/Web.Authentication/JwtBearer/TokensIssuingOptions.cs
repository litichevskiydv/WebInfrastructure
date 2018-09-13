namespace Skeleton.Web.Authentication.JwtBearer
{
    using System;
    using Microsoft.IdentityModel.Tokens;

    public class TokensIssuingOptions
    {
        public string GetEndpoint { get; set; }

        public SecurityKey SigningKey { get; set; }

        public string SigningAlgorithmName { get; set; }

        public TimeSpan? Lifetime { get; set; }

        public ITokenIssueEventHandler TokenIssueEventHandler { get; set; }
    }
}