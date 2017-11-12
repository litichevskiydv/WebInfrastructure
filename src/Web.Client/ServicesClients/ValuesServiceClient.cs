namespace Web.Client.ServicesClients
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Skeleton.Web.Conventions.Responses;
    using Skeleton.Web.Integration.BaseApiClient;
    using Skeleton.Web.Integration.BaseApiClient.Configuration;

    public class ValuesServiceClient : FlurlBasedClient
    {
        public ValuesServiceClient(Func<IClientConfigurator, IClientConfigurator> configurationBuilder) : base(configurationBuilder)
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

        public string Get(int id)
        {
            return Get<string>($"api/values/{id}");
        }

        public Task<string> GetAsync(int id)
        {
            return GetAsync<string>($"api/values/{id}");
        }

        public void Set(int id, string value)
        {
            Put($"api/values/{id}", value);
        }

        public Task SetAsync(int id, string value)
        {
            return PutAsync($"api/values/{id}", value);
        }

        public ApiResponse<int, ApiResponseError> Post(int id, string value)
        {
            return Post<ApiResponse<int, ApiResponseError>>($"api/values/{id}", value);
        }

        public Task<ApiResponse<int, ApiResponseError>> PostAsync(int id, string value)
        {
            return PostAsync<ApiResponse<int, ApiResponseError>>($"api/values/{id}", value);
        }

        public void Delete(int id)
        {
            Delete($"api/values/{id}");
        }

        public Task DeleteAsync(int id)
        {
            return DeleteAsync($"api/values/{id}");
        }
    }
}