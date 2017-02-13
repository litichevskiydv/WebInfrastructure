namespace Web.Client
{
    using System.Net.Http;
    using ServicesClients;
    using Skeleton.Web.Integration;

    public partial class ApiClient : BaseFluentClient
    {
        public ApiClient(ClientConfiguration configuration)
        {
            _valuesServiceClient = new ValuesServiceClient(configuration);
            ServicesClients.Add(_valuesServiceClient);

            _accountControllerClient = new AccountControllerClient(configuration);
            ServicesClients.Add(_accountControllerClient);
        }

        public ApiClient(ClientConfiguration configuration, HttpMessageHandler messageHandler)
        {
            _valuesServiceClient = new ValuesServiceClient(messageHandler, configuration);
            ServicesClients.Add(_valuesServiceClient);

            _accountControllerClient = new AccountControllerClient(messageHandler, configuration);
            ServicesClients.Add(_accountControllerClient);
        }
    }
}