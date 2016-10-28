namespace Web.Client.ServicesClients
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Skeleton.Web.Integration;

    public class ValuesServiceClient : BaseClient
    {
        public ValuesServiceClient(ClientConfiguration configuration) : base(configuration)
        {
        }

        public ValuesServiceClient(HttpMessageHandler messageHandler, ClientConfiguration configuration) : base(messageHandler, configuration)
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