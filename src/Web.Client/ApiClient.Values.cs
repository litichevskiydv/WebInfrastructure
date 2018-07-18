namespace Web.Client
{
    using System.Threading.Tasks;
    using Models.Input;
    using ServicesClients.ValuesService;

    public partial class ApiClient
    {
        private readonly ValuesServiceClient _valuesServiceClient;

        public ApiClient GetValues()
        {
            CurrentState = _valuesServiceClient.Get();
            return this;
        }

        public async Task<ApiClient> GetValuesAsync()
        {
            CurrentState = await _valuesServiceClient.GetAsync();
            return this;
        }

        public ApiClient GetValue(int id)
        {
            CurrentState = _valuesServiceClient.Get(id);
            return this;
        }

        public async Task<ApiClient> GetValueAsync(int id)
        {
            CurrentState = await _valuesServiceClient.GetAsync(id);
            return this;
        }

        public ApiClient SetValue(ValuesModificationRequest request)
        {
            _valuesServiceClient.Set(request);
            return this;
        }

        public async Task<ApiClient> SetValueAsync(ValuesModificationRequest request)
        {
            await _valuesServiceClient.SetAsync(request);
            return this;
        }

        public ApiClient PostValue(int id, string value)
        {
            CurrentState = _valuesServiceClient.Post(id, value);
            return this;
        }

        public async Task<ApiClient> PostValueAsync(int id, string value)
        {
            CurrentState = await _valuesServiceClient.PostAsync(id, value);
            return this;
        }

        public ApiClient DeleteValue(int id)
        {
            _valuesServiceClient.Delete(id);
            return this;
        }

        public async Task<ApiClient> DeleteValueAsync(int id)
        {
            await _valuesServiceClient.DeleteAsync(id);
            return this;
        }
    }
}