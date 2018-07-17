namespace Web.Client.ServicesClients
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Skeleton.Web.Authentication.JwtBearer.Models;
    using Skeleton.Web.Integration.BaseApiClient;

    public class AccountControllerClient : BaseClient
    {
        private string _token;

        public AccountControllerClient(HttpClient httpClient, IOptions<AccountControllerClientOptions> clientOptions) 
            : base(httpClient, clientOptions.Value)
        {
        }

        public void SetToken(string token = null)
        {
            _token = token;
        }

        protected override void RequestHeadersConfigurator(HttpRequestHeaders requestHeaders)
        {
            if (string.IsNullOrWhiteSpace(_token) == false)
                requestHeaders.WithBearerToken(_token);
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