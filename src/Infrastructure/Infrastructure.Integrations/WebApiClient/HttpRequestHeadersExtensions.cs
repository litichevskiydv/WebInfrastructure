namespace Infrastructure.Integrations.WebApiClient
{
    using System;
    using System.Net.Http.Headers;
    using System.Text;

    public static class HttpRequestHeadersExtensions
    {
        public static HttpRequestHeaders WithBearerToken(this HttpRequestHeaders headers, string token)
        {
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return headers;
        }

        public static HttpRequestHeaders WithBasicAuth(this HttpRequestHeaders headers, string username, string password)
        {
            var value = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            headers.Authorization = new AuthenticationHeaderValue("Basic", value);
            return headers;
        }
    }
}