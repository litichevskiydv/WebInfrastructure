namespace Web.Client.ServicesClients
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Infrastructure.Integrations.WebApiClient;

    public class ValuesServiceClient : BaseClient
    {
        public ValuesServiceClient(ClientConfiguration configuration) : base(configuration)
        {
        }

        public ValuesServiceClient(ClientConfiguration configuration, HttpMessageHandler messageHandler) : base(configuration, messageHandler)
        {
        }

        public IEnumerable<string> Get()
        {
            return Get<IEnumerable<string>>("api/values");
        }

        public Task<IEnumerable<string>> GetAsync()
        {
            return GetAsync<IEnumerable<string>>("api/values");
        } 
    }
}