namespace Skeleton.Web.Authorisation.JwtBearer.Models
{
    using System;

    public class TokenResponseModel
    {
        public string Token { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}