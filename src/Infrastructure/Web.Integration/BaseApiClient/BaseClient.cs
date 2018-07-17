namespace Skeleton.Web.Integration.BaseApiClient
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Configuration;
    using Conventions.Responses;
    using Exceptions;
    using Serialization.Abstractions;

    public abstract class BaseClient
    {
        private readonly HttpClient _httpClient;
        private readonly ISerializer _serializer;
        private readonly Dictionary<HttpStatusCode, Type> _exceptionsTypesByStatusCodes;

        protected virtual void RequestHeadersConfigurator(HttpRequestHeaders requestHeaders)
        {
        }

        protected BaseClient(HttpClient httpClient, BaseClientOptions options)
        {
            if(httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(options.BaseUrl))
                throw new ArgumentNullException(nameof(options.BaseUrl));
            if (options.Serializer == null)
                throw new ArgumentNullException(nameof(options.Serializer));

            _serializer = options.Serializer;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(options.BaseUrl);
            _httpClient.Timeout = options.Timeout;
            
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(_serializer.MediaType.MediaType)
            );

            if (string.IsNullOrWhiteSpace(_serializer.MediaType.CharSet) == false)
            {
                _httpClient.DefaultRequestHeaders.AcceptCharset.Clear();
                _httpClient.DefaultRequestHeaders.AcceptCharset.Add(
                    new StringWithQualityHeaderValue(_serializer.MediaType.CharSet)
                );
            }

            _exceptionsTypesByStatusCodes = new Dictionary<HttpStatusCode, Type>
                                            {
                                                {HttpStatusCode.NotFound, typeof(NotFoundException)},
                                                {HttpStatusCode.Unauthorized, typeof(UnauthorizedException)},
                                                {HttpStatusCode.BadRequest, typeof(BadRequestException)}
                                            };
        }

        private async Task<HttpResponseMessage> GetResponseMessageAsync(string url, HttpMethod method, object data)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            RequestHeadersConfigurator(_httpClient.DefaultRequestHeaders);

            var request = new HttpRequestMessage(method, url);
            if (method != HttpMethod.Get)
                request.Content = data as HttpContent ?? _serializer.Serialize(data);
            return await _httpClient.SendAsync(request).ConfigureAwait(false);
        }

        private HttpResponseMessage GetResponseMessage(string url, HttpMethod method, object data)
        {
            return GetResponseMessageAsync(url, method, data).GetAwaiter().GetResult();
        }

        private async Task<TResponse> ReadAsAsyncWithAwaitConfiguration<TResponse>(HttpContent httpContent)
        {
            using (var stream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false))
                return _serializer.Deserialize<TResponse>(stream);
        }

        private string ReadSpecialApiResponseMessage<TResponse>(Stream stream)
        {
            try
            {
                stream.Position = 0;
                var message = _serializer.Deserialize<TResponse>(stream)?.ToString();
                return string.IsNullOrWhiteSpace(message) ? null : message;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<Exception> CreateException(HttpResponseMessage responseMessage)
        {
            var exceptionMessage = responseMessage.ReasonPhrase;
            Exception errorResponseReadingException = null;
            if (responseMessage.Content != null)
            {
                string responseText = null;
                try
                {
                    using (var stream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        responseText =
                            ReadSpecialApiResponseMessage<ApiExceptionResponse>(stream)
                            ?? ReadSpecialApiResponseMessage<ApiResponse<object, ApiResponseError>>(stream)
                            ?? await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    errorResponseReadingException = exception;
                }
                if (string.IsNullOrWhiteSpace(responseText) == false)
                    exceptionMessage = responseText;
            }

            Type exceptionType;
            if (_exceptionsTypesByStatusCodes.TryGetValue(responseMessage.StatusCode, out exceptionType))
                return errorResponseReadingException == null
                    ? (Exception)Activator.CreateInstance(exceptionType, exceptionMessage)
                    : (Exception)Activator.CreateInstance(exceptionType, exceptionMessage, errorResponseReadingException);

            return errorResponseReadingException == null
                ? new ApiException(responseMessage.StatusCode, exceptionMessage)
                : new ApiException(responseMessage.StatusCode, exceptionMessage, errorResponseReadingException);
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

            throw CreateException(responseMessage).GetAwaiter().GetResult();
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