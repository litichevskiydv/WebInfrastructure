namespace Web.Client.ServicesClients
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Skeleton.Web.Authentication.JwtBearer.Models;
    using Skeleton.Web.Integration.BaseApiClient;

    public class AccountControllerClient : FlurlBasedClient
    {
        private string _token;

        public AccountControllerClient(ClientConfiguration configuration) : base(configuration)
        {
        }

        public AccountControllerClient(HttpMessageHandler messageHandler, ClientConfiguration configuration) : base(messageHandler, configuration)
        {
        }

        public void SetToken(string token = null)
        {
            _token = token;
        }

        protected override void ConfigureRequestHeaders(IDictionary<string, object> headers)
        {
            if (string.IsNullOrWhiteSpace(_token) == false)
                headers.WithBearerToken(_token);
        }

        public TokenResponseModel Token(string login, string password)
        {
            return Post<TokenResponseModel>("/api/Account/Token", new TokenRequestModel {Login = login, Password = password});
        }

        public Task<TokenResponseModel> TokenAsync(string login, string password)
        {
            return PostAsync<TokenResponseModel>("api/Account/Token", new TokenRequestModel {Login = login, Password = password});
        }

        public IEnumerable<string> UserInfo()
        {
            return Post<IEnumerable<string>>("api/Account/UserInfo", null);
        }

        public Task<IEnumerable<string>> UserInfoAsync()
        {
            return PostAsync<IEnumerable<string>>("/api/Account/UserInfo", null);
        }
    }
}