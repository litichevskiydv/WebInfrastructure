namespace Skeleton.Web.Authorisation.JwtBearer.Models
{
    using System.Collections.Generic;

    public class TokenRequestModel
    {
        public string Login { get; private set; }
        public string Password { get; private set; }
        public IEnumerable<KeyValuePair<string, string>> Content { get; private set; }

        public TokenRequestModel(string login, string password)
        {
            Login = login;
            Password = password;

            Content = new[]
                      {
                          new KeyValuePair<string, string>(nameof(Login), login),
                          new KeyValuePair<string, string>(nameof(Password), password)
                      };
        }
    }
}