namespace Infrastructure.Integrations.WebApiClient
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Exceptions;
    using Newtonsoft.Json;

    public abstract class BaseClient
    {
        private readonly string _baseUrl;
        private readonly TimeSpan _timeout;
        private readonly HttpMessageHandler _messageHandler;

        private readonly MediaTypeFormatter _formatter;

        protected virtual void RequestHeadersConfigurator(HttpRequestHeaders requestHeaders)
        {
        }

        protected BaseClient(ClientConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _baseUrl = configuration.BaseUrl;
            _timeout = TimeSpan.FromMilliseconds(configuration.TimeoutInMilliseconds);

            _formatter = new JsonMediaTypeFormatter
                         {
                             SerializerSettings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}
                         };
        }

        protected BaseClient(HttpMessageHandler messageHandler, ClientConfiguration configuration) : this(configuration)
        {
            if (messageHandler == null)
                throw new ArgumentNullException(nameof(messageHandler));

            _messageHandler = messageHandler;
        }

        private async Task<HttpResponseMessage> GetResponseMessageAsync(string url, HttpMethod method, object data)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            using (var httpClient = new HttpClient(_messageHandler ?? new HttpClientHandler(), _messageHandler == null))
            {
                httpClient.BaseAddress = new Uri(_baseUrl);
                httpClient.Timeout = _timeout;

                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", _formatter.SupportedMediaTypes.First().MediaType);
                foreach (var mediaTypeHeaderValue in _formatter.SupportedMediaTypes)
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeHeaderValue.MediaType));
                RequestHeadersConfigurator(httpClient.DefaultRequestHeaders);

                var request = new HttpRequestMessage(method, url);
                if (method != HttpMethod.Get && data != null)
                    request.Content = data as HttpContent ??
                                      new ObjectContent(data.GetType(), data, _formatter, _formatter.SupportedMediaTypes.FirstOrDefault());
                return await httpClient.SendAsync(request).ConfigureAwait(false);
            }
        }

        private HttpResponseMessage GetResponseMessage(string url, HttpMethod method, object data)
        {
            return GetResponseMessageAsync(url, method, data).Result;
        }

        private async Task<T> ReadAsAsyncWithAwaitConfiguration<T>(HttpContent httpContent)
        {
            return await httpContent.ReadAsAsync<T>(Enumerable.Repeat(_formatter, 1)).ConfigureAwait(false);
        }

        private static async Task<Exception> CreateException(HttpResponseMessage responseMessage)
        {
            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                return new NotFoundException(responseMessage.ReasonPhrase);
            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                return new UnauthorizedException(responseMessage.ReasonPhrase);
            if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestException(responseMessage.ReasonPhrase);

            if (responseMessage.Content != null)
                try
                {
                    var errorResponse = (await responseMessage.Content.ReadAsAsync<ApiErrorResponse>().ConfigureAwait(false)).ToString();
                    return new ApiException(responseMessage.StatusCode,
                               string.IsNullOrWhiteSpace(errorResponse) ? responseMessage.ReasonPhrase : errorResponse);
                }
                catch (Exception exception)
                {
                    return new ApiException(responseMessage.StatusCode, responseMessage.ReasonPhrase, exception);
                }
            return new ApiException(responseMessage.StatusCode, responseMessage.ReasonPhrase);
        }

        private async Task<T> CallAsync<T>(string url, HttpMethod method, object data)
        {
            var responseMessage = await GetResponseMessageAsync(url, method, data);
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    if (responseMessage.StatusCode == HttpStatusCode.NoContent)
                        return default(T);
                    return await ReadAsAsyncWithAwaitConfiguration<T>(responseMessage.Content);
                }
            }

            throw await CreateException(responseMessage);
        }

        private T Call<T>(string url, HttpMethod method, object data)
        {
            var responseMessage = GetResponseMessage(url, method, data);
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                    return responseMessage.StatusCode == HttpStatusCode.NoContent
                        ? default(T)
                        : ReadAsAsyncWithAwaitConfiguration<T>(responseMessage.Content).Result;
            }

            throw CreateException(responseMessage).Result;
        }

        private async Task CallAsync(string url, HttpMethod method, object data)
        {
            var responseMessage = await GetResponseMessageAsync(url, method, data);
            if (responseMessage != null && responseMessage.IsSuccessStatusCode)
                return;

            throw await CreateException(responseMessage);
        }

        private void Call(string url, HttpMethod method, object data)
        {
            var responseMessage = GetResponseMessage(url, method, data);
            if (responseMessage != null && responseMessage.IsSuccessStatusCode)
                return;

            throw CreateException(responseMessage).Result;
        }

        protected Task<T> GetAsync<T>(string url)
        {
            return CallAsync<T>(url, HttpMethod.Get, null);
        }

        protected T Get<T>(string url)
        {
            return Call<T>(url, HttpMethod.Get, null);
        }

        protected Task<T> PutAsync<T>(string url, object data)
        {
            return CallAsync<T>(url, HttpMethod.Put, data);
        }

        protected Task PutAsync(string url, object data)
        {
            return CallAsync(url, HttpMethod.Put, data);
        }

        protected T Put<T>(string url, object data)
        {
            return Call<T>(url, HttpMethod.Put, data);
        }

        protected void Put(string url, object data)
        {
            Call(url, HttpMethod.Put, data);
        }

        protected Task<T> PostAsync<T>(string url, object data)
        {
            return CallAsync<T>(url, HttpMethod.Post, data);
        }

        protected Task PostAsync(string url, object data)
        {
            return CallAsync(url, HttpMethod.Post, data);
        }

        protected void Post(string url, object data)
        {
            Call(url, HttpMethod.Post, data);
        }

        protected T Post<T>(string url, object data)
        {
            return Call<T>(url, HttpMethod.Post, data);
        }

        protected Task DeleteAsync(string url)
        {
            return CallAsync(url, HttpMethod.Delete, null);
        }

        protected void Delete(string url)
        {
            Call(url, HttpMethod.Delete, null);
        }
    }
}