namespace Web.Client.ServicesClients
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Models.Input;
    using Skeleton.Web.Conventions.Responses;
    using Skeleton.Web.Integration.BaseApiClient;

    public class ValuesServiceClient : BaseClient
    {
        public ValuesServiceClient(HttpClient httpClient, IOptions<ValuesServiceClientOptions> clientOptions) 
            : base(httpClient, clientOptions.Value)
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

        public void Set(ValuesModificationRequest request)
        {
            Put("api/values", request);
        }

        public Task SetAsync(ValuesModificationRequest request)
        {
            return PutAsync("api/values", request);
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