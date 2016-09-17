namespace Web.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Infrastructure.Integrations.WebApiClient;
    public class ValuesClient : BaseClient
    {
        public ValuesClient(ClientConfiguration configuration) : base(configuration)
        {
        }

        public ValuesClient(ClientConfiguration configuration, HttpMessageHandler messageHandler) : base(configuration, messageHandler)
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