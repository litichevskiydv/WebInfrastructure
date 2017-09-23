namespace Web.Client
{
    using System;
    using ServicesClients;
    using Skeleton.Web.Integration.BaseApiClient.Configuration;
    using Skeleton.Web.Integration.BaseApiFluentClient;

    public partial class ApiClient : BaseFluentClient
    {
        public ApiClient(Func<IClientConfigurator, IClientConfigurator> configurationBuilder)
        {
            _valuesServiceClient = new ValuesServiceClient(configurationBuilder);
            ServicesClients.Add(_valuesServiceClient);

            _accountControllerClient = new AccountControllerClient(configurationBuilder);
            ServicesClients.Add(_accountControllerClient);
        }
    }
}