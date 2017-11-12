namespace Skeleton.Web.Integration.BaseApiClient
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using Configuration;
    using Conventions.Responses;
    using Exceptions;
    using Flurl.Http;
    using Flurl.Http.Configuration;
    using Flurl.Http.Content;

    public abstract class FlurlBasedClient
    {
        private readonly string _baseUrl;
        private readonly ClientFlurlHttpSettings _clientSettings;

        private readonly Dictionary<HttpStatusCode, Type> _exceptionsTypesByStatusCodes;

        protected FlurlBasedClient(Func<IClientConfigurator, IClientConfigurator> configurationBuilder)
        {
            if (configurationBuilder == null)
                throw new ArgumentNullException(nameof(configurationBuilder));

            var clientConfigurator = new ClientConfigurator();
            configurationBuilder(clientConfigurator);
            _baseUrl = clientConfigurator.BaseUrl;
            _clientSettings = clientConfigurator.ClientSettings;

            _exceptionsTypesByStatusCodes =
                new Dictionary<HttpStatusCode, Type>
                {
                    {HttpStatusCode.NotFound, typeof(NotFoundException)},
                    {HttpStatusCode.Unauthorized, typeof(UnauthorizedException)},
                    {HttpStatusCode.BadRequest, typeof(BadRequestException)}
                };
        }

        protected virtual void ConfigureRequestHeaders(IDictionary<string, object> headers)
        {
        }

        private FlurlClient CreateClient()
        {
            var client = new FlurlClient(_baseUrl) {Settings = _clientSettings};
            ConfigureRequestHeaders(client.Headers);
            client.Headers["Accept"] = "application/json";

            return client;
        }

        private Exception CreateException(FlurlHttpException captureException)
        {
            if (captureException is FlurlHttpTimeoutException || captureException.Call.HttpStatus.HasValue == false)
                ExceptionDispatchInfo.Capture(captureException.InnerException).Throw();

            var exceptionMessage = captureException.Call.Response.ReasonPhrase;
            var errorResponseText = captureException.GetResponseString();
            if(string.IsNullOrWhiteSpace(errorResponseText) == false)
                try
                {
                    var deserializerMessage = captureException.GetResponseJson<ApiExceptionResponse>()?.ToString();
                    exceptionMessage = string.IsNullOrWhiteSpace(deserializerMessage) ? errorResponseText : deserializerMessage;
                }
                catch
                {
                    exceptionMessage = errorResponseText;
                }

            if (_exceptionsTypesByStatusCodes.TryGetValue(captureException.Call.Response.StatusCode, out var exceptionType))
                return (Exception)Activator.CreateInstance(exceptionType, exceptionMessage);
            return new ApiException(captureException.Call.Response.StatusCode, exceptionMessage);
        }

        private async Task<T> SendAsync<T>(string url, HttpMethod method, object data)
        {
            using (var client = CreateClient())
                try
                {
                    HttpContent requestContent = null;
                    if (method != HttpMethod.Get && data != null)
                        requestContent = data as HttpContent ?? new CapturedJsonContent(_clientSettings.JsonSerializer.Serialize(data));

                    return await url.WithClient(client).SendAsync(method, requestContent).ReceiveJson<T>();
                }
                catch (FlurlHttpException exception)
                {
                    throw CreateException(exception);
                }
        }

        private async Task SendAsync(string url, HttpMethod method, object data)
        {
            using (var client = CreateClient())
                try
                {
                    HttpContent requestContent = null;
                    if (method != HttpMethod.Get && data != null)
                        requestContent = data as HttpContent ?? new CapturedJsonContent(_clientSettings.JsonSerializer.Serialize(data));

                    await url.WithClient(client).SendAsync(method, requestContent);
                }
                catch (FlurlHttpException exception)
                {
                    throw CreateException(exception);
                }
        }

        protected Task<T> GetAsync<T>(string url)
        {
            return SendAsync<T>(url, HttpMethod.Get, null);
        }

        protected T Get<T>(string url)
        {
            return GetAsync<T>(url).GetAwaiter().GetResult();
        }

        protected Task<T> PutAsync<T>(string url, object data)
        {
            return SendAsync<T>(url, HttpMethod.Put, data);
        }

        protected Task PutAsync(string url, object data)
        {
            return SendAsync(url, HttpMethod.Put, data);
        }

        protected T Put<T>(string url, object data)
        {
            return PutAsync<T>(url, data).GetAwaiter().GetResult();
        }

        protected void Put(string url, object data)
        {
            PutAsync(url, data).GetAwaiter().GetResult();
        }

        protected Task<T> PostAsync<T>(string url, object data)
        {
            return SendAsync<T>(url, HttpMethod.Post, data);
        }

        protected Task PostAsync(string url, object data)
        {
            return SendAsync(url, HttpMethod.Post, data);
        }

        protected void Post(string url, object data)
        {
            PostAsync(url, data).GetAwaiter().GetResult();
        }

        protected T Post<T>(string url, object data)
        {
            return PostAsync<T>(url, data).GetAwaiter().GetResult();
        }

        protected Task DeleteAsync(string url)
        {
            return SendAsync(url, HttpMethod.Delete, null);
        }

        protected void Delete(string url)
        {
            DeleteAsync(url).GetAwaiter().GetResult();
        }
    }
}