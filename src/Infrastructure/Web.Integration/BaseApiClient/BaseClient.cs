namespace Skeleton.Web.Integration.BaseApiClient
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Configuration;
    using Conventions.Responses;
    using Exceptions;

    public abstract class BaseClient
    {
        private readonly ClientConfiguration _configuration;
        private readonly Dictionary<HttpStatusCode, Type> _exceptionsTypesByStatusCodes;

        protected virtual void RequestHeadersConfigurator(HttpRequestHeaders requestHeaders)
        {
        }

        protected BaseClient(Func<ClientConfiguration, ClientConfiguration> configurationBuilder)
        {
            if (configurationBuilder == null)
                throw new ArgumentNullException(nameof(configurationBuilder));

            _configuration = configurationBuilder(new ClientConfiguration());
            if(string.IsNullOrWhiteSpace(_configuration.BaseUrl))
                throw new ArgumentNullException(nameof(_configuration.BaseUrl));
            if (_configuration.Serializer == null)
                throw new ArgumentNullException(nameof(_configuration.Serializer));
            if (_configuration.HttpMessageHandlersFactory == null)
                throw new ArgumentNullException(nameof(_configuration.HttpMessageHandlersFactory));

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

            using (var httpClient = new HttpClient(_configuration.HttpMessageHandlersFactory(), _configuration.DisposeHttpMessageHandlersAfterCall))
            {
                httpClient.BaseAddress = new Uri(_configuration.BaseUrl);
                httpClient.Timeout = _configuration.Timeout;

                RequestHeadersConfigurator(httpClient.DefaultRequestHeaders);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(_configuration.Serializer.MediaType.MediaType));
                if (string.IsNullOrWhiteSpace(_configuration.Serializer.MediaType.CharSet) == false)
                {
                    httpClient.DefaultRequestHeaders.AcceptCharset.Clear();
                    httpClient.DefaultRequestHeaders.AcceptCharset.Add(
                        new StringWithQualityHeaderValue(_configuration.Serializer.MediaType.CharSet));
                }

                var request = new HttpRequestMessage(method, url);
                if (method != HttpMethod.Get && data != null)
                    request.Content = data as HttpContent ?? _configuration.Serializer.Serialize(data);
                return await httpClient.SendAsync(request).ConfigureAwait(false);
            }
        }

        private HttpResponseMessage GetResponseMessage(string url, HttpMethod method, object data)
        {
            return GetResponseMessageAsync(url, method, data).GetAwaiter().GetResult();
        }

        private async Task<T> ReadAsAsyncWithAwaitConfiguration<T>(HttpContent httpContent)
        {
            using (var stream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false))
                return _configuration.Serializer.Deserialize<T>(stream);
        }

        private async Task<ApiExceptionResponse> ReadApiExceptionResponse(HttpContent httpContent)
        {
            try
            {
                return await ReadAsAsyncWithAwaitConfiguration<ApiExceptionResponse>(httpContent);
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
                    responseText = (await ReadApiExceptionResponse(responseMessage.Content))?.ToString()
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