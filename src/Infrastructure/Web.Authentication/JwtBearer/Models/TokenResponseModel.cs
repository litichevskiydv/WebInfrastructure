namespace Skeleton.Web.Authentication.JwtBearer.Models
{
    using System;

    public class TokenResponseModel
    {
        public string Token { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}