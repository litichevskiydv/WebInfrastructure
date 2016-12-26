namespace Web.Client
{
    using System.Net.Http;
    using ServicesClients;
    using Skeleton.Web.Integration;

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

        public ApiClient GetValue(int id)
        {
            CurrentState = _valuesServiceClient.Get(id);
            return this;
        }

        public ApiClient SetValue(int id, string value)
        {
            _valuesServiceClient.Set(id, value);
            return this;
        }

        public ApiClient PostValue(int id, string value)
        {
            CurrentState = _valuesServiceClient.Post(id, value);
            return this;
        }

        public ApiClient DeleteValue(int id)
        {
            _valuesServiceClient.Delete(id);
            return this;
        }
    }
}