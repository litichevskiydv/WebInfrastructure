namespace Web.Client
{
    using System.Net.Http;
    using Infrastructure.Integrations.WebApiClient;
    using ServicesClients;

    public class ApiClient : BaseFluentClient
    {
        private readonly ValuesServiceClient _valuesServiceClient;

        public ApiClient(ClientConfiguration configuration)
        {
            _valuesServiceClient = new ValuesServiceClient(configuration);
            ServicesClients.Add(_valuesServiceClient);
        }

        public ApiClient(ClientConfiguration configuration, HttpMessageHandler messageHandler)
        {
            _valuesServiceClient = new ValuesServiceClient(messageHandler, configuration);
            ServicesClients.Add(_valuesServiceClient);
        }

        public ApiClient GetValues()
        {
            CurrentState = _valuesServiceClient.Get();
            return this;
        }
    }
}