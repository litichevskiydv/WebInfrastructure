namespace Web.Client.ServicesClients.ValuesService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Input;
    using Skeleton.Web.Conventions.Responses;

    public interface IValuesServiceClient
    {
        IEnumerable<string> Get();

        Task<IEnumerable<string>> GetAsync();

        string Get(int id);

        Task<string> GetAsync(int id);

        void Set(ValuesModificationRequest request);

        Task SetAsync(ValuesModificationRequest request);

        ApiResponse<int, ApiResponseError> Post(int id, string value);

        Task<ApiResponse<int, ApiResponseError>> PostAsync(int id, string value);

        void Delete(int id);

        Task DeleteAsync(int id);
    }
}