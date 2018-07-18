namespace Web.Client
{
    using ServicesClients.AccountController;

    public partial class ApiClient
    {
        private readonly AccountControllerClient _accountControllerClient;

        public ApiClient Login(string login, string password)
        {
            var response = _accountControllerClient.Token(login, password);
            CurrentState = response;
            _accountControllerClient.SetToken(response.Token);
            return this;
        }

        public ApiClient UserInfo()
        {
            CurrentState = _accountControllerClient.UserInfo();
            return this;
        }
    }
}